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
using EduClass.Web.Infrastructure.Mappers;
using System.Collections.Generic;
using MvcPaging;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private static IGroupServices _serviceGroup;
        private static IPersonServices _servicePerson;
        private const int defaultPageSize = 1;
        

        public GroupsController(IGroupServices serviceGroup, IPersonServices servicePerson)
        {
            _serviceGroup = serviceGroup;
            _servicePerson = servicePerson;
        }

        public ActionResult Index()
        {
            var list = _serviceGroup.GetAll().OrderBy(a => a.Id);

            return View(list);
        }

        [HttpGet]
        public ActionResult GetContacts(string student_name, int? page)
        {
            IList<Student> estudiantes = null;
            try
            {
                ViewData["student_name"] = student_name;
                var group = _serviceGroup.GetById(1);
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

                //estudiantes = estudiantes.ToPagedList(currentPageIndex, defaultPageSize);


            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
            }

            return View(estudiantes);
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
        public ActionResult JoinStudent([Bind(Include = "Key")] GroupViewModel groupVm)
        {

            try
            {
                Person student = null;
                int idStudent = UserSession.GetCurrentUser().Id;//Obtengo el usuario Actual

                var group = _serviceGroup.GetByKey(groupVm.Key);//Obtengo el Grupo del Id pasado por parametro
                student = _servicePerson.GetById(idStudent);

                if (group == null || student == null) { return HttpNotFound(); }

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
            }


            return RedirectToAction("JoinStudent", "Groups");
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
