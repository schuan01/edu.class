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
using log4net;


namespace EduClass.Web.Controllers
{
    [Authorize]
    public class CalendarsController : Controller
    {
        private static ICalendarServices _service;
        private ILog _log;

        public CalendarsController(ICalendarServices service, ILog log)
        {
           
            _service = service;
            _log = log;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.Id);

            return View(list);
        }

        // GET: Calendar
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CalendarViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description")]CalendarViewModel calendarVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        //Execute the mapping 
                        var calendar = AutoMapper.Mapper.Map<CalendarViewModel, Calendar>(calendarVm);

                        calendar.CreatedAt = DateTime.Now;
                        calendar.Enabled = true;

                        _service.Create(calendar);

                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", calendarVm.calendarName)));

                        return RedirectToAction("Index");
                    
                }
                catch (Exception ex)
                {
                    _log.Error("Calendar - Create -> ", ex);
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al crear usuario", typeof(calendarController), ex));
                }
            }

            return View(calendarVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var calendar = AutoMapper.Mapper.Map<Calendar, CalendarViewModel>(_service.GetById(id));

            return View(calendar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Description")]CalendarViewModel calendarVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var calendar = AutoMapper.Mapper.Map<CalendarViewModel, Calendar>(calendarVm);

                    calendar.UpdatedAt = DateTime.Now;

                    _service.Create(calendar);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", calendarVm.calendarName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _log.Error("Calendar - Edit -> ", ex);
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(calendarController), ex));
                }
            }

            return View(calendarVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var calendar = _service.GetById(id);

            if (calendar == null) { return HttpNotFound(); }

            if (calendar.Enabled) calendar.Enabled = false;
            else calendar.Enabled = true;

            calendar.UpdatedAt = DateTime.Now;

            _service.Update(calendar);


            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", calendar.Name)));

            return RedirectToAction("Index");
        }
    }
}


