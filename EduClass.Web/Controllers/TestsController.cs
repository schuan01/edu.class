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
            var list = _service.GetAll().OrderBy(a => a.Name);

            return View(list);
        }

        // GET: Test
        [HttpGet]
        public ActionResult Create()
        {
            return View(new TestViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Description, StartDate, EndDate")]TestViewModel testVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Se creara primero el test sin preguntas.
                    //Luego se editara el mismo.


                    //Execute the mapping 
                    var test = AutoMapper.Mapper.Map<TestViewModel, Test>(testVm);

                    test.GroupId = 1;//TODO GET CURRENT GROUP
                    test.CreatedAt = DateTime.Now;
                    test.Enabled = true;

                    if (UserSession.GetCurrentUser() is Teacher)
                        _service.Create(test);
                    else
                        throw new Exception("El usuario actual no es un Profesor");

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Creacion Exitosa", "El Test se creo correctamente"));

                    return RedirectToAction("Create");

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo crear el Test"));
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
        public ActionResult Edit([Bind(Include = "Name, Description, StartDate, EndDate, GroupId")]TestViewModel testVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var test = AutoMapper.Mapper.Map<TestViewModel, Test>(testVm);
                    test.UpdatedAt = DateTime.Now;

                    _service.Update(test);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", testVm.testName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(testController), ex));
                }
            }

            return View(testVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var test = _service.GetById(id);

            if (test == null) { return HttpNotFound(); }

            if (test.Enabled) test.Enabled = true;
            else test.Enabled = true;

            test.UpdatedAt = DateTime.Now;

            _service.Update(test);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", test.Name)));

            return RedirectToAction("Index");
        }
    }
}


