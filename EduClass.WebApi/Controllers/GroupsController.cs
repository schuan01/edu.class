using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Mailers;
using EduClass.WebApi.Infrastructure;
using EduClass.WebApi.Infrastructure.Sessions;
using EduClass.WebApi.Infrastructure.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
        private static ICalendarServices _calendarService;

        public GroupsController(IGroupServices service, IPersonServices personService, IPostServices postService, ICalendarServices calendarService)
        {
            _service = service;
            _personService = personService;
            _postService = postService;
            _calendarService = calendarService;


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
                    gr.Description = g.Description;
                    gr.Key = g.Key;
                    gr.Enabled = g.Enabled;
                    foreach (Student s in g.Students)
                    {
                        //AGREGO UN STUDENT VACIO SIMPLEMENTE PARA CONTARLO
                        Student se = new Student();
                        gr.Students.Add(se);
                    }
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

            return Json(new { idGrupo = g.Id, nombreGrupo = g.Name, descripcion = g.Description, claveGrupo = g.Key, estado = g.Enabled });
        }

        [HttpPost]
        [Route("JoinStudent")]
        [AllowAnonymous]
        public IHttpActionResult JoinStudent([FromBody]JObject parametros)
        {

            try
            {
                var usuario = parametros["usuario"].ToObject<int>();
                var clave = parametros["clave"].ToObject<string>();

                Person student = null;
                int idStudent = usuario;//Obtengo el usuario Actual

                var group = _service.GetByKey(clave);//Obtengo el Grupo del Id pasado por parametro
                student = _personService.GetById(idStudent);

                if (group == null || student == null) { throw new Exception("El usuario o el grupo no existe"); }

                if (group.Students.FirstOrDefault(st => st.Id == student.Id) != null)//Si ya existe en la collecion
                {
                    throw new Exception("El usuario actual ya existe en el grupo seleccionado");
                }

                if (student is Student)//Solo aplica si es tipo Student
                {
                    group.Students.Add((Student)student);
                    _service.Update(group);

                    return Json(new { idGrupo = group.Id, nombreGrupo = group.Name, descripcion = group.Description, claveGrupo = group.Key, estado = group.Enabled, mensaje = "El usuario actual se agrego correctamente", url = "Board.html" });
                }
                else
                    throw new Exception("El usuario actual no es un estudiante");

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
                //_log.Error("Groups - JoinStudent", ex);
            }
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public IHttpActionResult Create([FromBody]JObject parametros)
        {
            var nombre = parametros["nombre"].ToObject<string>();
            var descripcion = parametros["descripcion"].ToObject<string>();
            var usuario = parametros["usuario"].ToObject<int>();

            try
            {
                
                Group group = new Group();
                group.Name = nombre;
                group.Description = descripcion;
                group.CreatedAt = DateTime.Now;
                group.Enabled = true;

                Person teacher = _personService.GetById(usuario);

                if (teacher is Teacher && teacher != null)
                    group.Teacher = (Teacher)teacher;
                else
                    throw new Exception("El usuario actual no es un Profesor o no existe");


                group.Key = Security.EncodePasswordBase64().Substring(0, 8);

                _service.Create(group);

                var calendar = new Calendar()
                {
                    Description = string.Format("Calendar - {0}", group.Name),
                    Group = group,
                    CreatedAt = DateTime.Now,
                    Enabled = true
                };

                _calendarService.Create(calendar);

                return Json(new { idGrupo = group.Id, nombreGrupo = group.Name, descripcion = group.Description, claveGrupo = group.Key, estado = group.Enabled, mensaje = "El usuario actual se agrego correctamente", url = "GroupIndex.html" });
               


                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message }); ;
                    //_log.Error("Groups - Create", ex);
                }


            
        }

        [HttpPost]
        [Route("InviteStudentByMail")]
        [AllowAnonymous]
        public IHttpActionResult InviteStudentByMail(FormDataCollection formCollection)
        {
            try
            {
                String valores = formCollection["select6"];
                string idGrupo = formCollection["groupId"];
                string rutaServidor = ConfigurationManager.AppSettings["UrlServer"];

                List<string> mails = valores.Split(',').ToList<string>();
                mails.Reverse();

                var uMailer = new UserMailer();

                Group g = _service.GetById(Convert.ToInt32(idGrupo));
                if (g != null)
                {
                    //var urlGroup = Url.Link("InviteStudentByMail", new { key = g.Key});
                    var urlGroup = g.Key;
                    uMailer.InviteUserToGroup(mails, g, urlGroup).Send();
                    return Json(new { mensaje = "El mail fue enviado correctamente" });
                }
                else
                {
                    throw new Exception("El grupo seleccionado no existe");
                }

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
                
                //_log.Error("Groups - InviteStudentByMail", ex);
            }

           
        }

       /* [HttpGet]
        [Route("InviteStudentByMail", Name = "InviteStudentByMail")]
        [AllowAnonymous]
        public IHttpActionResult InviteStudentByMail(string key)
        {
            try
            {
                Person student = null;
                int idStudent = usuario;//Obtengo el usuario Actual

                var group = _service.GetByKey(key);//Obtengo el Grupo del Id pasado por parametro
                student = _personService.GetById(idStudent);

                if (group == null || student == null)
                {
                    throw new Exception("El grupo o el usuario no existe");
                }

                if (group.Students.FirstOrDefault(st => st.Id == student.Id) != null)//Si ya existe en la collecion
                {
                    throw new Exception("El usuario actual ya existe en el grupo seleccionado");
                }

                if (student is Student)//Solo aplica si es tipo Student
                {
                    group.Students.Add((Student)student);
                    _service.Update(group);

                    return Json(new { mensaje = "El usuario actual se agrego correctamente" });
                    
                }
                else
                    throw new Exception("El usuario actual no es un estudiante");

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
                //_log.Error("Groups - InviteStudentByMail", ex);
            }

            
        }*/


    }
}
