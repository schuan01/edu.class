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
    public class ReplysController : Controller
    {
        private static IReplyServices _service;

        public ReplysController(IReplyServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.Id);

            return View(list);
        }

        // GET: Reply
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ReplyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Description, PersonId, PostId")]ReplyViewModel replyVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        //Execute the mapping 
                        var reply = AutoMapper.Mapper.Map<ReplyViewModel, Reply>(replyVm);

                        reply.CreatedAt = DateTime.Now;
                        reply.Enabled = true;

                        _service.Create(reply);

                        //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", replyVm.replyName)));

                        return RedirectToAction("Index");
                  
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al crear usuario", typeof(replyController), ex));
                }
            }

            return View(replyVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var reply = AutoMapper.Mapper.Map<Reply, ReplyViewModel>(_service.GetById(id));

            return View(reply);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name, Description, PersonId, PostId")]ReplyViewModel replyVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var reply = AutoMapper.Mapper.Map<ReplyViewModel, Reply>(replyVm);

                    
                    reply.UpdatedAt = DateTime.Now;

                    _service.Update(reply);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", replyVm.replyName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(replyController), ex));
                }
            }

            return View(replyVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var reply = _service.GetById(id);

            if (reply == null) { return HttpNotFound(); }

            if (reply.Enabled == true) reply.Enabled = true;
            else reply.Enabled = true;

            reply.UpdatedAt = DateTime.Now;

            _service.Update(reply);


            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", reply.Name)));

            return RedirectToAction("Index");
        }
    }
}


