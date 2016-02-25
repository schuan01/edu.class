﻿using EduClass.Entities;
using EduClass.Logic;
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
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult SignIn(string datosUsu)
        {

            dynamic datos = JsonConvert.DeserializeObject<string>(datosUsu);

            string userName = datos.UserName;
            string password = datos.Password;

            if (!User.Identity.IsAuthenticated)
            {

                var user = _service.SignIn(userName, Security.EncodePassword(password));

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(userName, false);

                    UserSession.SetCurrentUser(user);

                    return Json(new { mensaje = "Bienvenido a EduClass", url = "Board.html" });
                }
                else
                {
                    return Json(new { error = "El usuario y la contraseña no cohinciden" });
                    
                }
            }
            else
            {
                return Json(new { mensaje = "Bienvenido a EduClass", url = "Board.html" });
            }

                      
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Register(PersonViewModel personVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    

                    //TODO: El chequeo debe ser con username y Email
                    if (_service.GetByUserName(personVm.UserName) == null)
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
                        person.Enabled = true;
                        person.Silenced = false;

                        _service.Create(person);

                        CreateUserFolder(person);
                        SetDefaultAvatar(person);

                        return Json(new { mensaje = "Se creo correcamente", url="SignIn.html" });
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
