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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using log4net;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private static IEventServices _service;
        private static IGroupServices _serviceGroup;
        private ILog _log;

        public EventsController(IEventServices service, IGroupServices serviceGroup, ILog log)
        {
            _service = service;
            _serviceGroup = serviceGroup;
            _log = log;
        }

        public ActionResult Index()
        {
            IOrderedEnumerable<Event> list = null;
            if (UserSession.GetCurrentGroup() != null)
            {
                Group g = _serviceGroup.GetById(UserSession.GetCurrentGroup().Id);
                if (g != null)
                {
                    list = g.Calendar.Events.Where(x => x.Enabled).OrderBy(a => a.Name);

                    ViewBag.EventTypeList = Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList();
                    ViewBag.CalendarId = _serviceGroup.GetById(UserSession.GetCurrentGroup().Id).Calendar.Id;
                }
                else
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No hay grupo seleccionado"));
                    _log.Error("Events - Index => No hay grupo seleccionado");
                    ViewBag.EventTypeList = Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList();
                    ViewBag.CalendarId = "";
                }

            }
            else
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER,"Error", "No hay grupo seleccionado"));
                _log.Error("Events - Index => No hay grupo seleccionado");
                ViewBag.EventTypeList = Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList();
                ViewBag.CalendarId = "";
            }

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrModifyEvent([Bind(Include = "Id, Title, Description, Start, End, EventType, CalendarId")]EventViewModel eventVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (eventVm.Id == 0)
                    {
                        //Execute the mapping 
                        var _event = AutoMapper.Mapper.Map<EventViewModel, Event>(eventVm);
 
                        _event.CreatedAt = DateTime.Now;
                        _event.Enabled = true;

                        _service.Create(_event);
                    }
                    else
                    {
                        var eEvent = _service.GetById(eventVm.Id);

                        var _event = AutoMapper.Mapper.Map<EventViewModel, Event>(eventVm, eEvent);

                        _event.UpdateAt = DateTime.Now;
                        _event.Enabled = true;

                        _service.Update(_event);
                    }

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Éxito", string.Format("El evento {0} fue creado con éxito", eventVm.Title)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al crear evento"));
                    _log.Error("Events - AddOrModifyEvent", ex);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var Event = _service.GetById(id);

            if (Event == null) { return HttpNotFound(); }

            if (Event.Enabled) Event.Enabled = false;
            else Event.Enabled = true;

            

            _service.Update(Event);

            MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Éxito", "El evento fue deshabilitado con éxito"));

            return RedirectToAction("Index");
        }

        public JsonResult GetAllEvents(int idCalendar = 0) 
        {
            if (idCalendar == 0) { return Json("Error", JsonRequestBehavior.AllowGet); }
            
            var eventList = _service.GetAll().Where(x=> x.CalendarId == idCalendar).OrderBy(a => a.Name);

            var returnedList = (from e in eventList
                                select new {
                                    id = e.Id,
                                    title = e.Name,
                                    description = e.Description,
                                    start = e.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                                    end = e.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                                    eventType = e.EventType.ToString(),
                                    calendarId = e.CalendarId
                                });

            return Json(returnedList, JsonRequestBehavior.AllowGet);
        }
    }
}


