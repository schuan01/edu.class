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

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        private static IPageServices _service;

        public PagesController(IPageServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.Id);

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
        public ActionResult Create([Bind(Include = "Title, Content, BoardId")]PageViewModel pageVm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Execute the mapping 
                    var page = AutoMapper.Mapper.Map<PageViewModel, Page>(pageVm);

                    page.CreatedAt = DateTime.Now;
                    page.Enabled = true;
                    page.GroupId = 1;//TODO GET CURRENT Group
                    page.Content = HttpUtility.HtmlEncode(page.Content);

                    _service.Create(page);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Creacion Exitosa", "La Página se creo correctamente"));

                    return RedirectToAction("Create");

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo crear la Página"));
                }
            }

            return View(pageVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var page = AutoMapper.Mapper.Map<Page, PageViewModel>(_service.GetById(id));

            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title, Content, BoardId")]PageViewModel pageVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var page = AutoMapper.Mapper.Map<PageViewModel, Page>(pageVm);

                    
                    page.UpdatedAt = DateTime.Now;

                    _service.Create(page);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", pageVm.pageName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(pageController), ex));
                }
            }

            return View(pageVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var page = _service.GetById(id);

            if (page == null) { return HttpNotFound(); }

            if (page.Enabled) page.Enabled = true;
            else page.Enabled = true;

            page.UpdatedAt = DateTime.Now;

            _service.Update(page);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", page.Name)));

            return RedirectToAction("Index");
        }
    }
}


