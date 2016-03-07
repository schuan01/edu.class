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

        public GroupsController(IGroupServices service, IPersonServices personService)
        {
            _service = service;
            _personService = personService;


        }

        [HttpGet]
        [Route("GetGroups")]
        [AllowAnonymous]
        public IHttpActionResult GetGroups(int id)
        {
            Person p = _personService.GetById(id);

            List<Group> grupos = new List<Group>();
            if (p is Teacher)
            {
                grupos = ((Teacher)p).Group.ToList();
            }

            List <Group> gruposSencillo = new List<Group>();
            foreach (Group g in grupos)
            {
                Group gr = new Group();
                gr.Id = g.Id;
                gr.Name = g.Name;
                gruposSencillo.Add(gr);
            }
            return Json(JsonConvert.SerializeObject(gruposSencillo));
        }

        [HttpPost]
        [Route("ChangeGroup")]
        [AllowAnonymous]
        public IHttpActionResult ChangeGroup(int grupo)
        {
            //Group g = _service.GetById(grupo.Id);
            Group g = _service.GetById(grupo);
            if (g != null)
            {
                Person p = _personService.GetById(UserSession.GetCurrentUser().Id);
                if (p is Teacher)
                {
                    //Buscamos si el grupo seleccionado forma parte del Usuario
                    if (((Teacher)p).Group.FirstOrDefault(gr => gr.Id == g.Id) == null)
                    {
                        throw new Exception("El usuario seleccionado no forma parte de este grupo ");
                    }
                }
                else if (p is Student)
                {
                    //Buscamos si el grupo seleccionado forma parte del Usuario
                    if (((Student)p).Groups.FirstOrDefault(gr => gr.Id == g.Id) == null)
                    {
                        throw new Exception("El usuario seleccionado no forma parte de este grupo ");
                    }
                }

                //Si sale todo OK, seteo el Current Group
                UserSession.SetCurrentGroup(g);
            }
            else
            {
                throw new Exception("El grupo actual no existe");
            }

            return Json(new { idGrupo = g.Id, nombreGrupo = g.Name});
        }
    }
}
