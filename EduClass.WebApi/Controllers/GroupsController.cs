using EduClass.Entities;
using EduClass.Logic;
using EduClass.WebApi.Infrastructure.Sessions;
using EduClass.WebApi.Infrastructure.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EduClass.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Groups")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
       
        private static IGroupServices _service;
        private static IPersonServices _personService;
        private static IPostServices _postService;

        public GroupsController(IGroupServices service, IPersonServices personService, IPostServices postService)
        {
            _service = service;
            _personService = personService;
            _postService = postService;


        }

        [HttpGet]
        [Route("GetGroups")]
        [AllowAnonymous]
        public IHttpActionResult GetGroups(int id)
        {
            List<Group> gruposSencillo = new List<Group>();
            try
            {
                Person p = _personService.GetById(id);

                List<Group> grupos = new List<Group>();
                if (p is Teacher)
                {
                    grupos = ((Teacher)p).Group.ToList();
                }
                else
                {
                    grupos = ((Student)p).Groups.ToList();
                }


                foreach (Group g in grupos)
                {
                    Group gr = new Group();
                    gr.Id = g.Id;
                    gr.Name = g.Name;
                    gruposSencillo.Add(gr);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "Ocurrio un error en la solicitud" });
            }
            return Json(JsonConvert.SerializeObject(gruposSencillo));
        }

        [HttpPost]
        [Route("ChangeGroup")]
        [AllowAnonymous]
        public IHttpActionResult ChangeGroup([FromBody]JObject parametros)
        {
            var usuario = parametros["usuario"].ToObject<int>();
            var idGrupo = parametros["grupo"].ToObject<int>();

            //Group g = _service.GetById(grupo.Id);
            Group g = _service.GetById(idGrupo);
            if (g != null)
            {
                Person p = _personService.GetById(usuario);
                if (p is Teacher)
                {
                    //Buscamos si el grupo seleccionado forma parte del Usuario
                    if (((Teacher)p).Group.FirstOrDefault(gr => gr.Id == g.Id) == null)
                    {
                        return Json(new { error = "El usuario seleccionado no forma parte de este grupo " });
                        
                    }
                }
                else if (p is Student)
                {
                    //Buscamos si el grupo seleccionado forma parte del Usuario
                    if (((Student)p).Groups.FirstOrDefault(gr => gr.Id == g.Id) == null)
                    {
                        return Json(new { error = "El usuario seleccionado no forma parte de este grupo " });
                        

                    }
                }

                //Si sale todo OK, seteo el Current Group
                UserSession.SetCurrentGroup(g);
            }
            else
            {
                return Json(new { error = "El grupo actual no existe" });
            }

            return Json(new { idGrupo = g.Id, nombreGrupo = g.Name});
        }

        
    }
}
