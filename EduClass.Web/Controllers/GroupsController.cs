using EduClass.Logic;
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
using EduClass.Web.Infrastructure.Mappers;
using System.Collections.Generic;
using MvcPaging;
using EduClass.Web.Mailers;
using log4net;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private static IGroupServices _serviceGroup;
        private static IPersonServices _servicePerson;
        private static ICalendarServices _serviceCalendar;
        private static IEventServices _serviceEvent;
        private static IPostServices _servicePost;
        private static ICalificationServices _serviceCalification;
        private static IPageServices _servicePage;
        private static ITestServices _serviceTest;
        private ILog _log;
        private const int defaultPageSize = 10;
        


        public GroupsController(IGroupServices serviceGroup, IPersonServices servicePerson, ICalendarServices serviceCalendar, IEventServices serviceEvent, IPostServices servicePost, ICalificationServices serviceCalification, IPageServices servicePage, ITestServices serviceTest, ILog log)
        {
            _serviceGroup = serviceGroup;
            _servicePerson = servicePerson;
            _serviceCalendar = serviceCalendar;
            _serviceEvent = serviceEvent;
            _servicePost = servicePost;
            _serviceCalification = serviceCalification;
            _servicePage = servicePage;
            _serviceTest = serviceTest;
            _log = log;
        }

        public ActionResult Index()
        {
            
            List<Group> grupos = new List<Group>();
            if (UserSession.GetCurrentUser() is Teacher)//Si el que ingresa es un teacher
            {
                //Obtengo los grupos en los que esta el Teacher.
                Teacher t = (Teacher)_servicePerson.GetById(UserSession.GetCurrentUser().Id);
                grupos = t.Group.ToList();//Todos los grupos
            }
            
            return View(grupos);
        }
        [HttpPost]
        public ActionResult ChangeGroup(int id)
        {
            try
            {
                Group g = _serviceGroup.GetById(id);
                if (g != null)
                {
                    Person p = _servicePerson.GetById(UserSession.GetCurrentUser().Id);
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
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - ChangeGroup", ex);
            }

            return RedirectToAction("Index", "Groups");//Esto es al pedo porque la llamda es por AJAX
        }
        [HttpGet]
        public ActionResult GetContacts(string student_name, int? page)
        {
            IList<Student> estudiantes = new List<Student>();
            try
            {
                if (UserSession.GetCurrentGroup() != null)
                {
                    ViewData["student_name"] = student_name;
                    var group = _serviceGroup.GetById(UserSession.GetCurrentGroup().Id);
                    ViewBag.StudentsGroup = group.Students;
                    ViewBag.TeacherGroup = group.Teacher;

                    int currentPageIndex = page.HasValue ? page.Value : 1;
                    estudiantes = group.Students.ToList();

                    if (string.IsNullOrWhiteSpace(student_name))
                    {
                        estudiantes = estudiantes.ToPagedList(currentPageIndex, defaultPageSize);
                    }
                    else
                    {
                        estudiantes = estudiantes.Where(p => p.FirstName.ToLower().Contains(student_name.ToLower())).ToPagedList(currentPageIndex, defaultPageSize);
                    }
                    
                    ViewBag.nombreTeacher = group.Teacher.FirstName + " " + group.Teacher.LastName;
                    ViewBag.urlTeacher = group.Teacher.Avatar.UrlPhoto;
                }
                else
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No hay grupo seleccionado"));
                    estudiantes = estudiantes.ToPagedList(1, defaultPageSize);
                    ViewBag.nombreTeacher = "";
                    ViewBag.urlTeacher = "";
                    _log.Error("Groups - GetContacts => No hay grupo seleccionado");

                }


            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - GetContacts", ex);
            }

            
            
            return View(estudiantes);
            
        }

       

        // GET: Group
        [HttpGet]
        public ActionResult JoinStudent()
        {
            return View(new GroupViewModel());
        }

        [HttpPost]
        public ActionResult JoinStudent([Bind(Include = "Key")] GroupViewModel groupVm)
        {

            try
            {
                Person student = null;
                int idStudent = UserSession.GetCurrentUser().Id;//Obtengo el usuario Actual

                var group = _serviceGroup.GetByKey(groupVm.Key);//Obtengo el Grupo del Id pasado por parametro
                student = _servicePerson.GetById(idStudent);

                if (group == null || student == null) { throw new Exception("El usuario o el grupo no existe"); }

                if (group.Students.FirstOrDefault(st => st.Id == student.Id) != null)//Si ya existe en la collecion
                {
                    throw new Exception("El usuario actual ya existe en el grupo seleccionado");
                }

                if (student is Student)//Solo aplica si es tipo Student
                {
                    group.Students.Add((Student)student);
                    _serviceGroup.Update(group);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Unirse", "El usuario actual se agrego correctamente"));
                }
                else
                    throw new Exception("El usuario actual no es un estudiante");

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - JoinStudent", ex);
            }


            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public ActionResult InviteStudentByMail(string key)
        {
            try
            {
                Person student = null;
                int idStudent = UserSession.GetCurrentUser().Id;//Obtengo el usuario Actual

                var group = _serviceGroup.GetByKey(key);//Obtengo el Grupo del Id pasado por parametro
                student = _servicePerson.GetById(idStudent);

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
                    _serviceGroup.Update(group);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Unirse", "El usuario actual se agrego correctamente"));
                }
                else
                    throw new Exception("El usuario actual no es un estudiante");

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - InviteStudentByMail", ex);
                return RedirectToAction("Index", "Board");
            }

            return View();
        }

        [HttpPost]
        public ActionResult InviteStudentByMail(FormCollection formCollection)
        {
            try
            {
                String valores = formCollection["select6"];
                string idGrupo = formCollection["groupId"];
                List<string> mails = valores.Split(',').ToList<string>();
                mails.Reverse();

                var uMailer = new UserMailer();

                Group g = _serviceGroup.GetById(Convert.ToInt32(idGrupo));
                if (g != null)
                {
                    var urlGroup = Url.Action("InviteStudentByMail", "Groups", new { key = g.Key }, Request.Url.Scheme);
                    uMailer.InviteUserToGroup(mails, g, urlGroup).Send();
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Invitación Enviada", "Se ha enviado la invitación correctamente"));
                }
                else
                {
                    throw new Exception("El grupo seleccionado no existe");
                }

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - InviteStudentByMail", ex);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            
            return View(new GroupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Description")]GroupViewModel groupVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var group = AutoMapper.Mapper.Map<GroupViewModel, Group>(groupVm);
                    group.CreatedAt = DateTime.Now;
                    group.Enabled = true;

                    Person teacher = _servicePerson.GetById(UserSession.GetCurrentUser().Id);

                    if (teacher is Teacher && teacher != null)
                        group.Teacher = (Teacher)teacher;
                    else
                        throw new Exception("El usuario actual no es un Profesor o no existe");

                   
                    group.Key = Security.EncodePasswordBase64().Substring(0,8);

                    _serviceGroup.Create(group);
                    
                    var calendar = new Calendar() { 
                        Description = string.Format("Calendar - {0}", group.Name),
                        Group = group,
                        CreatedAt = DateTime.Now,
                        Enabled = true
                    };

                    _serviceCalendar.Create(calendar);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Grupo", "El grupo fue creado correctamente"));
                    return RedirectToAction("Index", "Groups");
                    

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                    _log.Error("Groups - Create", ex);
                }
            }

            return View(groupVm);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            GroupViewModel group = null;
            try
            {
                ViewBag.PersonType = UserSession.GetCurrentUser();
                if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                group = AutoMapper.Mapper.Map<Group, GroupViewModel>(_serviceGroup.GetById(id));

                
            }
            catch(Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - Edit", ex);
            }

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name, Description, Id")]GroupViewModel groupVm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var group = _serviceGroup.GetById(groupVm.Id);

                    group.Name = groupVm.Name;
                    group.Description = groupVm.Description;
                    group.UpdatedAt = DateTime.Now;

                    //Verifico si el Teacher que esta Logeado pertenece al Grupo
                    if (group.Teacher.Id != UserSession.GetCurrentUser().Id)
                    {
                        throw new Exception("El Profesor actual no pertenece a este grupo");
                    }

                    _serviceGroup.Update(group);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Grupo", "El grupo fue editado correctamente"));
                    return RedirectToAction("Index", "Groups");
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                    _log.Error("Groups - Edit", ex);
                }
            }

            return View(groupVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disable(int groupId = 0)
        {
            try
            {
                if (groupId == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                var group = _serviceGroup.GetById(groupId);

                //Verifico si el Teacher que esta Logeado pertenece al Grupo
                if (group.Teacher.Id != UserSession.GetCurrentUser().Id)
                {
                    throw new Exception("El Profesor actual no pertenece a este grupo");
                }


                if (group == null) { return HttpNotFound(); }

                if (group.Enabled) group.Enabled = false;
                else group.Enabled = true;

                group.UpdatedAt = DateTime.Now;

                _serviceGroup.Update(group);

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Grupo", "El grupo fue editado correctamente"));

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - Disable", ex);
            }
            return RedirectToAction("Index");
        }


        //ELIMINARA TOTALMENTE EL GRUPO, Y CUALQUIER RELACION CON EL
        //NO SE IMPLEMENTARA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGroup(int groupId = 0)
        {
            try
            {
                //////////////////////////////////////////////
                //NO SE IMPLENTARA EN ESTA VERSION
                /////////////////////////////////////////////
                if (groupId == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                var group = _serviceGroup.GetById(groupId);

                //Verifico si el Teacher que esta Logeado pertenece al Grupo
                if (group.Teacher.Id != UserSession.GetCurrentUser().Id)
                {
                    throw new Exception("El Profesor actual no pertenece a este grupo");
                }


                if (group == null) { return HttpNotFound(); }

                List<int> idEventos = new List<int>();
                //BORRO EVENTOS Y CALENDARIO
                if (group.Calendar != null)
                {
                    foreach (var a in group.Calendar.Events)
                    {
                        idEventos.Add(a.Id);
                    }

                    foreach (int id in idEventos)
                    {
                        Event e = group.Calendar.Events.FirstOrDefault(x => x.Id == id);
                        group.Calendar.Events.Remove(e);
                        _serviceEvent.Delete(_serviceEvent.GetById(e.Id));
                    }

                    _serviceCalendar.Delete(group.Calendar);
                }


                List<int> idFiles = new List<int>();
                List<int> idPosts = new List<int>();
                List<int> idReplys = new List<int>();
                //BORRO ARCHIVOS DE LOS POSTS
                foreach (var a in group.Posts)
                {
                    foreach (var b in a.Files)
                    {
                        idFiles.Add(b.Id);
                    }

                    foreach (int i in idFiles)
                    {
                        a.Files.Remove(a.Files.FirstOrDefault(x => x.Id == i));
                    }

                    idPosts.Add(a.Id);
                }

                //BORRO LOS POSTS
                foreach (int i in idPosts)
                {
                    Post p = _servicePost.GetById(i);
                    foreach (var reply in p.Replays)
                    {
                        idReplys.Add(reply.Id);
                    }
                    foreach (int a in idReplys)
                    {
                        p.Replays.Remove(p.Replays.FirstOrDefault(x => x.Id == a));
                    }
                    group.Posts.Remove(group.Posts.FirstOrDefault(x => x.Id == i));
                    _servicePost.Delete(p);
                }

                //BORRO LAS CALIFICACIONES
                List<int> idCalificaciones = new List<int>();
                foreach(Calification i in group.Califications)
                {
                    idCalificaciones.Add(i.Id);
                }

                foreach (int i in idCalificaciones)
                {
                    group.Califications.Remove(group.Califications.FirstOrDefault(x => x.Id == i));
                    _serviceCalification.Delete(_serviceCalification.GetById(i));
                    
                }

                //BORRO LAS PAGINAS
                List<int> idPaginas = new List<int>();
                foreach (Page i in group.Pages)
                {
                    idPaginas.Add(i.Id);
                }

                foreach (int i in idPaginas)
                {
                    group.Pages.Remove(group.Pages.FirstOrDefault(x => x.Id == i));
                    _servicePage.Delete(_servicePage.GetById(i));
                }

                //BORRO LOS ALUMNOS, SOLO DE LA COLECCION
                List<int> idAlumnos = new List<int>();
                foreach (Student i in group.Students)
                {
                    idAlumnos.Add(i.Id);
                }

                foreach (int i in idAlumnos)
                {
                    group.Students.Remove(group.Students.FirstOrDefault(x => x.Id == i));
                    
                }

                //TODO
                //BORRO LOS TESTS
                /*
                List<int> idTests = new List<int>();
                List<int> idPreguntas = new List<int>();
                List<int> idRespuestas = new List<int>();
                foreach (Page i in group.Pages)
                {
                    idTests.Add(i.Id);
                }

                foreach (int i in idTests)
                {
                    Test t = _serviceTest.GetById(i);
                    foreach (var a in t.Questions)
                    {
                        idPreguntas.Add(a.Id);
                        foreach (var b in a.Response)
                        {
                            idRespuestas.Add(b.Id);
                        }

                        foreach (int c in idRespuestas)
                        {
                            a.Response.Remove(a.Response.FirstOrDefault(x => x.Id == c));

                        }
                    }
                }*/

                _serviceGroup.Delete(group);


                //HAGO ESTO SOLO PARA QUE SE CARGUE LA SESSION CON EL SIGUIENTE GRUPO DISPONIBLE
                var grupos = _serviceGroup.GetActiveGroups(UserSession.GetCurrentUser());
                if(grupos.Count > 0)
                    UserSession.SetCurrentGroup(grupos[0]);//Obtengo el primer grupo que venga y lo coloco en la Session

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Grupo", "El grupo fue eliminado correctamente"));

                //////////////////////////////////////////////
                //NO SE IMPLENTARA EN ESTA VERSION
                /////////////////////////////////////////////

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Groups - DeleteGroup", ex);
            }
            return RedirectToAction("Index");
        }
    }
}
