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
    public class TestsController : Controller
    {
        private static ITestServices _service;

        public TestsController(ITestServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            if (UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
            if (UserSession.GetCurrentGroup() == null) { return View(); }

            var list = _service.GetAll(UserSession.GetCurrentGroup().Id).OrderByDescending(a => a.CreatedAt);

            return View(list);
        }

        // GET: Test
        [HttpGet]
        public ActionResult Create()
        {
            var test = new TestViewModel();
            test.GroupId = UserSession.GetCurrentGroup().Id;
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Description, StartDate, EndDate, GroupId")]TestViewModel testVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var test = AutoMapper.Mapper.Map<TestViewModel, Test>(testVm);

                    test.GroupId = UserSession.GetCurrentGroup().Id;
                    test.CreatedAt = DateTime.Now;
                    test.Enabled = false; //Se pone deshabilitada para que no les aparezcan a los alumnos

                    if (UserSession.GetCurrentUser() is Teacher)
                        _service.Create(test);
                    else
                        throw new Exception("El usuario actual no es un Profesor");

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Creacion Exitosa", "La prueba se creo correctamente. <br />Haz clic en el icono [ <i class=\"fa fa-question\" style=\"font-size: 25px\"></i> ] para agregar preguntas a la prueba."));

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo crear la prueba"));
                }
            }

            return View(testVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var test = AutoMapper.Mapper.Map<Test, TestViewModel>(_service.GetById(id));

            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Description, StartDate, EndDate, GroupId")]TestViewModel testVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _service.GetById(testVm.Id);

                    var test = AutoMapper.Mapper.Map<TestViewModel, Test>(testVm, entity);
                    test.UpdatedAt = DateTime.Now;

                    _service.Update(test);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Test modificado", string.Format("El test {0} fue modificado con éxito", testVm.Name)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar test"));
                }
            }

            return View(testVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var test = _service.GetById(id);

            if (test == null) { return HttpNotFound(); }

            if (test.Enabled)
            {
                test.Enabled = false;
            }
            else 
            { 
                test.Enabled = true; 
            }

            test.UpdatedAt = DateTime.Now;

            _service.Update(test);

            MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Test modificado", string.Format("Se ha agregado una alerta a los alumnos para que comienzen el test.", test.Name)));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ReadyToTest(int id)
        { 
            if (UserSession.GetCurrentUser() is Teacher) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            return View(_service.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadyToTest(FormCollection frm)
        {
            if (UserSession.GetCurrentUser() is Teacher) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            return View();
            /*return View(_service.GetById(id));*/
        }
    }
}


