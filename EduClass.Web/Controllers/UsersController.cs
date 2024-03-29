﻿using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using EduClass.Web.Infrastructure;
using EduClass.Web.Infrastructure.ViewModels;
using EduClass.Entities;
using System.Data.Entity.Validation;
using EduClass.Web.Mailers;
using System.IO;
using System.Web;
using log4net;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private static IPersonServices _service;
        private static IAvatarServices _avatarService;
        private string carpetaRaiz = "~/UsersFolders/";//Carpeta raiz, no incluye los Avatars
        private ILog _log;

        public UsersController(IPersonServices service, IAvatarServices avatarService, ILog log)
        {
            _service = service;
            _avatarService = avatarService;
            _log = log;

        }

        // GET: User
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl = "", string userName = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(returnUrl)) { return Redirect(returnUrl); }

                return RedirectToAction("Index", "Board");
            }

            ViewBag.UserName = userName;
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string userName, string password, string returnurl)
        {
            if (!User.Identity.IsAuthenticated)
            {

                var user = _service.SignIn(userName, Security.EncodePassword(password));

                if (user != null)
                {
                    if (user.Enabled)
                    {
                        FormsAuthentication.SetAuthCookie(userName, false);

                        UserSession.SetCurrentUser(user);

                        //Prevent Redirection attack
                        if (!String.IsNullOrEmpty(returnurl) && Url.IsLocalUrl(returnurl))
                            return Redirect(returnurl);

                        return RedirectToAction("Index", "Board");
                    }
                    else
                    {
                        ViewBag.UserName = userName;
                        ViewBag.ReturnUrl = returnurl;
                        ViewBag.Message = " El usuario no está habilitado.";
                        _log.Error("Users - SignIn => Usuario no habilitado");
                    }
                }
                else
                {
                    ViewBag.UserName = userName;
                    ViewBag.ReturnUrl = returnurl;
                    ViewBag.Message = "El usuario y la contraseña no coinciden.";
                    _log.Error("Users - SignIn => Wrong Credentials");
                }
            }
            else
            {
                //Prevent Redirection attack
                if (!String.IsNullOrEmpty(returnurl) && Url.IsLocalUrl(returnurl))
                    return Redirect(returnurl);

                return RedirectToAction("SignIn", "Users");
            }

            return View();
        }

        public ActionResult SignOut()
        {
            var user = User.Identity.Name;

            FormsAuthentication.SignOut();
            UserSession.ClearSession();

            return RedirectToAction("SignIn", new { userName = user });
        }

        [HttpGet]
        public ActionResult ChangePassword(int id = 0)
        {
            if (id == 0) { id = UserSession.GetCurrentUser().Id; }

            var user = _service.GetById(id);

            var userMapped = new ChangePasswordViewModel()
            {
                PersonId = user.Id.ToString()
            };

            return View(userMapped);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "PersonId, OldPassword, NewPassword, ConfirmNewPassword")]ChangePasswordViewModel value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(value.PersonId);
                    var user = _service.GetById(userId);

                    if (user != null && Security.EncodePassword(value.OldPassword).Equals(user.Password))
                    {
                        _service.ChangePassword(userId, Security.EncodePassword(value.NewPassword));

                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Cambio de contraseña", "Su contraseña fue modificada con éxito."));

                        return RedirectToAction("Me", "Users");
                    }
                    else
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al ingresar la contraseña", "La contraseña actual no es valida."));
                        _log.Error("Users - ChangePassword => La contraseña actual no es valida");
                    }
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, ex.Message, "Error al cambiar la contraseña"));
                    _log.Error("Users - ChangePassword",ex);

                    return View(value);
                }
            }

            value.OldPassword = string.Empty;
            value.NewPassword = string.Empty;
            value.ConfirmNewPassword = string.Empty;

            return View(value);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new PersonViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserName, Email, Password, ConfirmPassword, FirstName, LastName, Birthday, IdentificationCard, PersonType")]PersonViewModel personVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (_service.GetByUserNameAndMail(personVm.UserName, personVm.Email) == null)
                    {
                        Person person;

                        if (personVm.PersonType.ToLower() == "teacher") {
                            person = AutoMapper.Mapper.Map<PersonViewModel, Teacher>(personVm); //person = new Teacher(); 
                        }
                        else {
                            person = AutoMapper.Mapper.Map<PersonViewModel, Student>(personVm); //person = new Student(); 
                        }
                        
                        person.CreatedAt = DateTime.Now;
                        person.Enabled = false;//Sea crea siempre inactivo, falta activar por MAIL
                        person.Silenced = false;

                        _service.Create(person);

                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", personVm.UserName)));

                        CreateUserFolder(person);
                        SetDefaultAvatar(person);

                        var uMailer = new UserMailer();
 
                        var urlPerson = Url.Action("EnableByMail", "Users", new { person = person.Id }, Request.Url.Scheme);
                        uMailer.Welcome(person,urlPerson).Send();

                        return View("_ActivationAccount");
                    }
                    else
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al crear usuario", string.Format("El usuario {0} o mail {1} ya existe", personVm.UserName,personVm.Email)));
                        _log.Error("Users - Register => El usuario o mail ya existe");
                    }
                }
                catch (DbEntityValidationException dex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear usuario, por favor contacte con el Administrador."));
                    _log.Error("Users - Register",dex);
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear usuario, por favor contacte con el Administrador."));
                    _log.Error("Users - Register", ex);
                }
            }

            return View(personVm);
        }


        [HttpGet]
        public ActionResult Edit()
        {
            PersonViewModel user = null;
            try
            {
                Person usuario = UserSession.GetCurrentUser();
                if(usuario == null)
                    throw new Exception("El usuario no existe");

                user = AutoMapper.Mapper.Map<Person, PersonViewModel>(_service.GetById(usuario.Id));

                
            }
            catch(Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Users - Edit", ex);
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName, FirstName, LastName, Birthday, Email, IdentificationCard")]PersonViewModel userVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var user =_service.GetById(userVm.Id);
                    user.FirstName = userVm.FirstName;
                    user.LastName = userVm.LastName;
                    user.Birthday = Convert.ToDateTime(userVm.Birthday);
                    user.Email = userVm.Email;
                    user.IdentificationCard = userVm.IdentificationCard;
                    user.UpdatedAt = DateTime.Now;

                    _service.Update(user);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", userVm.UserName)));

                    return RedirectToAction("Me");
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario"));
                    _log.Error("Users - Edit", ex);
                }
            }

            return View(userVm);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult EnableByMail(int person = 0)
        {
            if (person == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var user = _service.GetById(person);

            if (user == null) { return HttpNotFound(); }

            if (!user.Enabled)
            {
                user.Enabled = true;
            }
            else
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.INFO, "ACTIVADO", "Su cuenta ya esta activa"));
                _log.Error("Users - EnableByMail => Su cuenta ya esta activa");
                return RedirectToAction("SignIn");
            }
           
            user.UpdatedAt = DateTime.Now;
            _service.Update(user);
            
            MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario Activado", string.Format("El usuario {0} fue activado con éxito", user.UserName)));

            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public ActionResult Disable(int id = 0)
        {
            try
            {
                if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                var user = _service.GetById(id);

                if (user == null) { return HttpNotFound(); }


                if (user.Enabled) user.Enabled = false;

                user.UpdatedAt = DateTime.Now;

                _service.Update(user);

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", user.UserName)));
                return RedirectToAction("SignOut");
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Users - Disable", ex);
            }
            return RedirectToAction("Index", "Board");
        }

        //Silencia o quita el silencio
        //TODO Hay un tema con la relacion, el alumno solo deberia ser silenciado del grupo del profesor que lo silencio.
        //Hoy esta silenciando toda la cuenta
        [HttpPost]
        public ActionResult SilenceStudent(int idStudent)
        {

            try
            {
                Person s = _service.GetById(idStudent);
                if (s is Student)
                {
                    if (s.Silenced)
                        s.Silenced = false;
                    else
                        s.Silenced = true;

                    _service.Update(s);
                    if(s.Silenced)
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Silenciado", "Alumno silenciado con éxito"));
                    else
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Aceptado", "El alumno ya no se encuentra silenciado"));
                }


            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Users - SilenceStudent", ex);
            }

            return RedirectToAction("GetContacts", "Groups");

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordEmail()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return View();
            }

            var user = _service.GetByEmail(email);

            if (user != null)
            {
                var uMailer = new UserMailer();

                var newPassword = Security.EncodePasswordBase64().Substring(0, 8);

                _service.ChangePassword(user.Id, Security.EncodePassword(newPassword));

                uMailer.PasswordReset(email, newPassword).Send();

                ViewBag.Sended = true;
            }
            else
            {
                ViewBag.Sended = false;
            }
            
            return View();
        }

		[HttpGet]
        public ActionResult Me()//Accede al Perfil actual
        {
            //Actualizo la Session por las dudas
            Person p = _service.GetById(UserSession.GetCurrentUser().Id);
            UserSession.SetCurrentUser(p);

            return View();
        }

        [HttpGet]
        public ActionResult ChangeAvatar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeAvatar(int id= 0)
        {
            HttpPostedFileBase file = null;
            bool isSavedSuccessfully = true;
            string fName = "";
            string path = "";
            try
            {
               
               
                foreach (string fileName in Request.Files)
                {
                    file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        //La carpeta es el userName del usuario
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Content\\", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "avatars");
                        pathString = System.IO.Path.Combine(pathString, UserSession.GetCurrentUser().UserName);

                        var fileName1 = Path.GetFileName(file.FileName);
                        

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        path = string.Format("{0}\\{1}", pathString, file.FileName);
                        
                        file.SaveAs(path);


                        //Despues de guardar la imagen
                        Person p = UserSession.GetCurrentUser();//Traigo el usuario actual
                        p = _service.GetById(p.Id);

                        //Y creo un nuevo Avatar
                        Avatar a = new Avatar();
                        a.Enabled = true;
                        a.UrlPhoto = "~\\Content\\avatars\\" + UserSession.GetCurrentUser().UserName+"\\"+file.FileName;
                        a.UpdatedAt = DateTime.Now;
                        a.Person = p;
                        

                        //Termino creando el Avatar
                        if (p.Avatar == null)
                        {
                            _avatarService.Create(a);
                            UserSession.SetCurrentUser(p);//Actualizo la Session
                        }
                        else
                        {
                            //Si ya existe, elimino la anterior. Relacion 1 - 1. Tambien elimino fisicamente si no es Default
                            if(p.Avatar.UrlPhoto != "~\\Content\\images\\default.png")
                                System.IO.File.Delete(Server.MapPath(p.Avatar.UrlPhoto));

                            _avatarService.Delete(p.Avatar);
                            

                            _avatarService.Create(a);//Nuevo Avatar para la Person
                            UserSession.SetCurrentUser(p);//Actualizo la Session
                        }
                        

                    }

                }

                
            }
            catch(Exception ex)
            {
                //Borro el archivo fisico si se llega a crear
                if (file != null)
                {
                    FileInfo fi1 = new FileInfo(path);
                    fi1.Delete();
                }

                 MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al actualizar el Avatar."));
                _log.Error("Users - ChangeAvatar", ex);

            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error al guardar el archivo" });
            }
        }

        private void CreateUserFolder(Person person)
        {
            string carpetaUsuario = carpetaRaiz + person.UserName;
            if (!Directory.Exists(Server.MapPath(carpetaUsuario)))
            {
                Directory.CreateDirectory(Server.MapPath(carpetaUsuario));
            }
        }

        private void SetDefaultAvatar(Person p)
        {
            Avatar a = new Avatar();
            a.Enabled = true;
            a.UrlPhoto = "~\\Content\\images\\default.png";
            a.UpdatedAt = DateTime.Now;
            a.Person = p;
            //Termino creando el Avatar
            if (p.Avatar == null)
            {
                _avatarService.Create(a);

            }

        }


    }
}


