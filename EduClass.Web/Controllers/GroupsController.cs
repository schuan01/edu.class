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

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private static IGroupServices _serviceGroup;

        public GroupsController(IGroupServices serviceGroup)
        {
            _serviceGroup = serviceGroup;
        }

        public ActionResult Index()
        {
            var list = _serviceGroup.GetAll().OrderBy(a => a.Id);

            return View(list);
        }

        // GET: Group
        [HttpGet]
        [AllowAnonymous]
        public ActionResult JoinStudent()
        {
            return View(new GroupViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult JoinStudent([Bind(Include = "Name, Descripction, KeyId")] GroupViewModel groupVm)
        {
            Person student = null; 
            student = UserSession.GetCurrentUser();//Obtengo el usuario Actual
            

            var group = _serviceGroup.GetById(1);//Obtengo el Grupo del Id pasado por parametro
            
            if (group == null || student == null) { return HttpNotFound(); }

            if (group.Students.First(st => st.Id == student.Id) != null)//Si ya existe en la collecion
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "El usuario actual ya existe en el grupo seleccionado"));
            }
            
            if (student is Student )//Solo aplica si es tipo Student
                group.Students.Add((Student)student);
            else
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "El usuario actual no es un Estudiante"));



            return View();
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

                    Person teacher = UserSession.GetCurrentUser();

                    if (teacher is Teacher)
                        group.Teacher = (Teacher)teacher;
                    else
                        throw new Exception("El usuario actual no es un Profesor");
                    
                    group.Key = Security.EncodePasswordBase64(group.Name + group.Id.ToString()).Substring(0, 8);
                    
                    _serviceGroup.Create(group);


                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Grupo", "El grupo fue creado correctamente"));
                    return RedirectToAction("Create", "Groups");

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                }
            }

            return View(groupVm);
        }

       

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var group = AutoMapper.Mapper.Map<Group, GroupViewModel>(_serviceGroup.GetById(id));

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name, Descripction")]GroupViewModel groupVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var group = AutoMapper.Mapper.Map<GroupViewModel, Group>(groupVm);

                    group.UpdatedAt = DateTime.Now;

                    _serviceGroup.Create(group);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", groupVm.groupName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(groupController), ex));
                }
            }

            return View(groupVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var group = _serviceGroup.GetById(id);

            if (group == null) { return HttpNotFound(); }

            if (group.Enabled) group.Enabled = false;
            else group.Enabled = true;

            group.UpdatedAt = DateTime.Now;

            _serviceGroup.Update(group);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", group.Name)));

            return RedirectToAction("Index");
        }
    }
}
