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
using System.Web;
using System.Collections.Generic;
using log4net;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        private static IPageServices _service;
        private ILog _log;

        public PagesController(IPageServices service, ILog log)
        {
            _service = service;
            _log = log;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().Where(x => x.GroupId == UserSession.GetCurrentGroup().Id).OrderByDescending(a => a.Id);

            //Las páginas publicas del grupo actual y solo muestro las activas si es Student
            if (UserSession.GetCurrentUser() is Student)
                list = list.Where(x => x.Enabled).OrderByDescending(a => a.Id);
            
            foreach (var v in list)
            {
                v.Content = HttpUtility.HtmlDecode(v.Content);
            }
            return View(list);
        }



        // GET: Page
        [HttpGet]
        public ActionResult Create()
        {
            return View(new PageViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Content")]PageViewModel pageVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (UserSession.GetCurrentUser() is Student)
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No tiene permisos para crear una Página Publica"));
                        return RedirectToAction("Index");
                    }


                    //Execute the mapping 
                    var page = AutoMapper.Mapper.Map<PageViewModel, Page>(pageVm);

                    page.CreatedAt = DateTime.Now;
                    page.Enabled = true;
                    page.GroupId = UserSession.GetCurrentGroup().Id;
                    page.Content = HttpUtility.HtmlEncode(page.Content);

                    _service.Create(page);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Creacion Exitosa", "La Página se creo correctamente"));

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo crear la Página"));
                    _log.Error("Pages - Create", ex);
                }
            }

            return View(pageVm);
        }

        [HttpGet]
        public ActionResult ViewPage(int pageId = 0)
        {
            PageViewModel page = null;
            try
            {
                if (pageId == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                Page p = _service.GetById(pageId);
                if (p.GroupId != UserSession.GetCurrentGroup().Id)
                {
                    throw new Exception("La página no pertence al grupo actual");
                }

                if (p.Enabled == false)
                {
                    throw new Exception("No se puede visualizar esta página");

                }

                page = AutoMapper.Mapper.Map<Page, PageViewModel>(_service.GetById(pageId));
                page.Content = HttpUtility.HtmlDecode(page.Content);
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Pages - ViewPage", ex);
                return RedirectToAction("Index");

            }

            return View(page);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var page = AutoMapper.Mapper.Map<Page, PageViewModel>(_service.GetById(id));
            page.Content = HttpUtility.HtmlDecode(page.Content);

            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Content")]PageViewModel pageVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var page = _service.GetById(pageVm.Id);
                    page.Name = pageVm.Name;
                    page.Content = HttpUtility.HtmlEncode(page.Content);

                    if (page.GroupId != UserSession.GetCurrentGroup().Id)
                    {
                        throw new Exception("La página no pertence al grupo actual");
                    }

                    page.UpdatedAt = DateTime.Now;
                    _service.Update(page);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Página", "Página modificada con éxito"));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                    _log.Error("Pages - Edit", ex);
                }
            }

            return View(pageVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disable(int id = 0)
        {
            try
            {
                if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                var page = _service.GetById(id);

                if (page == null) { return HttpNotFound(); }

                if (page.Enabled) page.Enabled = false;
                else page.Enabled = true;

                page.UpdatedAt = DateTime.Now;

                _service.Update(page);

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Página creada", "Página modificada con éxito"));
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al modifica la página"));
                _log.Error("Pages - Disable", ex);
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                var page = _service.GetById(id);

                if (page == null)
                {
                    throw new Exception("La Página seleccionada no existe");
                }

                

                _service.Delete(page);

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Página Eliminada", "Página eliminada con éxito"));
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar la página"));
                _log.Error("Pages - Delete", ex);
            }


            return RedirectToAction("Index");
        }


    }
}


