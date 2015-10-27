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
        private static IGroupServices _service;

        public GroupsController(IGroupServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.Id);

            return View(list);
        }

        // GET: Group
        [HttpGet]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Join(Person _person)
        {
            return null;//TODO
        }




        [HttpGet]
        public ActionResult Create()
        {
            return View(new GroupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Descripction, KeyId")]GroupViewModel groupVm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Execute the mapping 
                    var group = AutoMapper.Mapper.Map<GroupViewModel, Group>(groupVm);
                    group.CreatedAt = DateTime.Now;
                    group.Enabled = true;

                    _service.Create(group);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario creado", string.Format("El usuario {0} fue creado con éxito", groupVm.groupName)));

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al crear usuario", typeof(groupController), ex));
                }
            }

            return View(groupVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var group = AutoMapper.Mapper.Map<Group, GroupViewModel>(_service.GetById(id));

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name, Descripction, KeyId")]GroupViewModel groupVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var group = AutoMapper.Mapper.Map<GroupViewModel, Group>(groupVm);

                    group.UpdatedAt = DateTime.Now;

                    _service.Create(group);

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

            var group = _service.GetById(id);

            if (group == null) { return HttpNotFound(); }

            if (group.Enabled) group.Enabled = false;
            else group.Enabled = true;

            group.UpdatedAt = DateTime.Now;

            _service.Update(group);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", group.Name)));

            return RedirectToAction("Index");
        }
    }
}
