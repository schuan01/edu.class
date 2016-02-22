using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class CalificationsController : Controller
    {

        private static ICalificationServices _service;
        private static IPersonServices _personService;
        private static IGroupServices _groupService;

        public CalificationsController(ICalificationServices service, IPersonServices personService, IGroupServices groupService)
        {
            _service = service;
            _personService = personService;
            _groupService = groupService;
        }


        // GET: Califications
        public ActionResult Index()
        {
            return View();
        }

        // GET: Page
        [HttpGet]
        public ActionResult Edit()
        {
            return View(new CalificationViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Oral,Test,Average,StudentId")]CalificationViewModel calficationVm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Execute the mapping 
                    var calification = AutoMapper.Mapper.Map<CalificationViewModel, Calification>(calficationVm);

                    calification.CreatedAt = DateTime.Now;
                    calification.GroupId = UserSession.GetCurrentGroup().Id;
                    

                    _service.Create(calification);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Creacion Exitosa", "La Calificacion se creo correctamente"));

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo crear la Calificacion"));
                }
            }

            return View(calficationVm);
        }
    }
}