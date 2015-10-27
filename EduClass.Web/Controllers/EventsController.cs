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
    public class EventsController : Controller
    {
        private static IEventServices _service;

        public EventsController(IEventServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.Name);

            return View(list);
        }

        // GET: Event
        [HttpGet]
        public ActionResult Create()
        {
            return View(new EventViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Description, Date, EventTypeId")]EventViewModel eventVm) {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        //Execute the mapping 
                        var Event = AutoMapper.Mapper.Map<EventViewModel, Event>(eventVm);

                        
                        Event.CreatedAt = DateTime.Now;
                        Event.Enabled = true;

                        _service.Create(Event);

                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", eventVm.EventName)));

                        return RedirectToAction("Index");
                   
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al crear usuario", typeof(EventController), ex));
                }
            }

            return View(eventVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var Event = AutoMapper.Mapper.Map<Event, EventViewModel>(_service.GetById(id));

            return View(Event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name, Description, Date, EventTypeId")]EventViewModel eventVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var Event = AutoMapper.Mapper.Map<EventViewModel, Event>(eventVm);
                   

                    _service.Create(Event);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", eventVm.EventName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(EventController), ex));
                }
            }

            return View(eventVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var Event = _service.GetById(id);

            if (Event == null) { return HttpNotFound(); }

            if (Event.Enabled) Event.Enabled = true;
            else Event.Enabled = false;

            

            _service.Update(Event);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", Event.Name)));

            return RedirectToAction("Index");
        }
    }
}


