﻿using EduClass.Logic;
using EduClass.Web.Infrastructure.Modules;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using EduClass.Web.Infrastructure;
using EduClass.Web.Infrastructure.ViewModels;
using EduClass.Entities;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private static IPersonServices _service;

        public UsersController(IPersonServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.UserName);

            return View(list);
        }

        // GET: User
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl = "", string userName = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(returnUrl)) { return Redirect(returnUrl); }

                return RedirectToAction("Index", "Home");
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
                    FormsAuthentication.SetAuthCookie(userName, false);

                    UserSession.SetCurrentUser(user);

                    //Prevent Redirection attack
                    if (!String.IsNullOrEmpty(returnurl) && Url.IsLocalUrl(returnurl))
                        return Redirect(returnurl);

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ViewBag.UserName = userName;
                    ViewBag.ReturnUrl = returnurl;
                    ViewBag.Message = "El usuario y la contraseña no cohinciden.";
                }
            }
            else
            {
                //Prevent Redirection attack
                if (!String.IsNullOrEmpty(returnurl) && Url.IsLocalUrl(returnurl))
                    return Redirect(returnurl);

                return RedirectToAction("Index", "User");
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

                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Cambio de contraseña", "Su contraseña fue modificada con éxito."));

                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al ingresar la contraseña", "La contraseña actual no es valida."));
                    }
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, ex.Message, "Error al cambiar la contraseña"));

                    return View(value);
                }
            }

            value.OldPassword = string.Empty;
            value.NewPassword = string.Empty;
            value.ConfirmNewPassword = string.Empty;

            return View(value);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new PersonViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName, Email, Password, ConfirmPassword, FirstName, LastName, Birthday, IdentificationCard")]PersonViewModel userVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_service.GetByUserName(userVm.UserName) == null)
                    {
                        //Execute the mapping 
                        var user = AutoMapper.Mapper.Map<PersonViewModel, Person>(userVm);

                        user.Password = Security.EncodePassword(user.Password);
                        user.CreatedAt = DateTime.Now;
                        user.Enabled = true;

                        _service.Create(user);

                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", userVm.UserName)));

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al crear usuario", string.Format("El usuario {0} ya existe", userVm.UserName)));
                    }
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al crear usuario", typeof(UserController), ex));
                }
            }

            return View(userVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var user = AutoMapper.Mapper.Map<Person, PersonViewModel>(_service.GetById(id));

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, UserName, FirstName, LastName, Birthday, Email, IdentificationCard")]PersonViewModel userVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var user = AutoMapper.Mapper.Map<PersonViewModel, Person>(userVm);

                    user.Password = Security.EncodePassword(user.Password);
                    user.UpdatedAt = DateTime.Now;

                    _service.Create(user);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", userVm.UserName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(UserController), ex));
                }
            }

            return View(userVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var user = _service.GetById(id);

            if (user == null) { return HttpNotFound(); }

            if (user.Enabled) user.Enabled = false;
            else user.Enabled = true;

            user.UpdatedAt = DateTime.Now;

            _service.Update(user);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", user.Name)));

            return RedirectToAction("Index");
        }
    }
}


