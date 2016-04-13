using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Mailers;
using EduClass.WebApi.Infrastructure;
using EduClass.WebApi.Infrastructure.Sessions;
using EduClass.WebApi.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;

using System.Web.Http;
using System.Web.Http.Cors;

using System.Web.Security;

namespace EduClass.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private string carpetaRaiz = "UsersFolders";//Carpeta raiz, no incluye los Avatars
        private static IPersonServices _service;
        private static IAvatarServices _avatarService;

        public UsersController(IPersonServices service, IAvatarServices avatarService)
        {
            _service = service;
            _avatarService = avatarService;


        }
 
        [HttpPost]
        [AllowAnonymous]
        [Route("SignIn")]
        public IHttpActionResult SignIn(PersonViewModel person)
        {

            string userName = person.UserName;
            string password = person.Password;

            if (!User.Identity.IsAuthenticated)
            {

                var user = _service.SignIn(userName, Security.EncodePassword(password));

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(userName, false);

                    UserSession.SetCurrentUser(user);

                    string tipo = "";
                    if (user is Teacher)
                        tipo = "Profesor";
                    else
                        tipo = "Estudiante";
                    
                    return Json(new { idUsuario = user.Id, usuario = user.UserName, nombre = user.FirstName, apellido = user.LastName, cumpleanios = user.Birthday, tipoUsuario = tipo, email = user.Email, identificacion = user.IdentificationCard, urlPhoto = user.Avatar.UrlPhoto, url = "Board.html" });
                }
                else
                {
                    return Json(new { error = "El usuario y la contraseña no coinciden" });
                    
                }
            }
            else
            {
                return Json(new { mensaje = "Bienvenido a EduClass", url = "Board.html" });
            }

                      
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(PersonViewModel personVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_service.GetByUserNameAndMail(personVm.UserName, personVm.Email) == null)
                    {
                        Person person;

                        if (personVm.PersonType.ToLower() == "teacher")
                        {
                            person = AutoMapper.Mapper.Map<PersonViewModel, Teacher>(personVm); //person = new Teacher(); 
                        }
                        else
                        {
                            person = AutoMapper.Mapper.Map<PersonViewModel, Student>(personVm); //person = new Student(); 
                        }

                        person.CreatedAt = DateTime.Now;
                        person.Enabled = false;//Sea crea siempre inactivo, falta activar por MAIL
                        person.Silenced = false;

                        _service.Create(person);

                        CreateUserFolder(person);
                        SetDefaultAvatar(person);

                        var uMailer = new UserMailer();
                        var urlPerson = person.Id.ToString();
                        uMailer.Welcome(person, urlPerson).Send();

                        return Json(new { mensaje = "Se le ha enviado un correo para activar su cuenta", url="SignIn.html" });
                    }
                    else
                    {
                        return Json(new { error = "El usuario y contraseña no coinciden" });
                    }
                }
                catch (DbEntityValidationException dex)
                {
                    return Json(new { error = dex.Message });
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }

            return Json(new { error = "Error al crear el usuario" });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPasswordEmail")]
        public IHttpActionResult ResetPasswordEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return Json(new { error = "Ingrese un mail valido" });
            }

            var user = _service.GetByEmail(email);

            if (user != null)
            {
                var uMailer = new UserMailer();

                var newPassword = Security.EncodePasswordBase64().Substring(0, 8);

                _service.ChangePassword(user.Id, Security.EncodePassword(newPassword));

                uMailer.PasswordReset(email, newPassword).Send();

                return Json(new { mensaje = "Se le ha enviado un correo con la contraseña nueva", url = "SignIn.html" });
            }
            else
            {
                return Json(new { error = "El mail ingresado no existe" });
            }

            
        }


        private void CreateUserFolder(Person person)
        {
            //REDIRECCIONA A LA CARPETA DEL WEB, DONDE ESTARIA TODO
            string carpetaUsuario = carpetaRaiz +"\\" +  person.UserName;
            string rutaServidor = ConfigurationManager.AppSettings["ServerPath"];
            string originalDirectory = new DirectoryInfo(string.Format("{0}\\"+carpetaUsuario, rutaServidor)).ToString();

            if (!Directory.Exists(originalDirectory))
            {
                Directory.CreateDirectory(originalDirectory);
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
