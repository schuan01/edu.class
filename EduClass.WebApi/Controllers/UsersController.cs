using EduClass.Entities;
using EduClass.Logic;
using EduClass.WebApi.Infrastructure;
using EduClass.WebApi.Infrastructure.Sessions;
using EduClass.WebApi.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace EduClass.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private string carpetaRaiz = "~/UsersFolders/";//Carpeta raiz, no incluye los Avatars
        private static IPersonServices _service;
        private static IAvatarServices _avatarService;

        public UsersController(IPersonServices service, IAvatarServices avatarService)
        {
            _service = service;
            _avatarService = avatarService;


        }

        /*[HttpPost]
        [AllowAnonymous]
        public IHttpActionResult SignIn(string userName, string password, string returnurl)
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



                    return RedirectToAction("Index", "Board");
                }
                else
                {
                    ViewBag.UserName = userName;
                    ViewBag.ReturnUrl = returnurl;
                    ViewBag.Message = "El usuario y la contraseña no coinciden.";
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
        }*/

        public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
            {
                if (actionExecutedContext.Response != null)
                    actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                base.OnActionExecuted(actionExecutedContext);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableCors(origins: "192.168.66.27", headers: "*", methods: "*")]
        [AllowCrossSiteJson]
        public System.Web.Mvc.JsonResult Register(PersonViewModel personVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //PersonViewModel personVm = new JavaScriptSerializer().Deserialize<PersonViewModel>(viewModelPersonAddress);

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

                        _service.Create(person);

                       //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", personVm.UserName)));

                        CreateUserFolder(person);
                        SetDefaultAvatar(person);

                        return new System.Web.Mvc.JsonResult()
                        {
                            Data = person,
                            JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al crear usuario", string.Format("El usuario {0} ya existe", personVm.UserName)));
                    }
                }
                catch (DbEntityValidationException dex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear usuario, por favor contacte con el Administrador."));
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear usuario, por favor contacte con el Administrador."));
                }
            }

            return new System.Web.Mvc.JsonResult()
            {
                Data = null,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }

        private void CreateUserFolder(Person person)
        {
            string carpetaUsuario = carpetaRaiz + person.UserName;
            if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(carpetaUsuario)))
            {
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(carpetaUsuario));
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
