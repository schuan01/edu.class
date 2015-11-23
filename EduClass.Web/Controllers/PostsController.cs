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
    public class PostsController : Controller
    {
        private static IPostServices _service;
        enum PostType
        {
            TEXT,
            LINK,
            PHOTO,
            FILE
        }

        public PostsController(IPostServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll().OrderBy(a => a.Title);
            return View(list);
        }

        // GET: Post
        [HttpGet]
        public ActionResult Create()
        {
            
            return View(new PostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title, Content")]PostViewModel postVm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Execute the mapping 
                    var post = AutoMapper.Mapper.Map<PostViewModel, Post>(postVm);

                    post.CreatedAt = DateTime.Now;
                    post.Enabled = true;

                    _service.Create(post);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Post", "El Post se ha creado correctamente"));

                    return RedirectToAction("Index");
                  
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Ocurrio un error al crear el Post"));
                }
            }

            return View(postVm);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var post = AutoMapper.Mapper.Map<Post, PostViewModel>(_service.GetById(id));

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title, Content, PersonId, GroupId")]PostViewModel postVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Execute the mapping 
                    var post = AutoMapper.Mapper.Map<PostViewModel, Post>(postVm);

                    
                    post.UpdatedAt = DateTime.Now;

                    _service.Update(post);

                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", postVm.postName)));

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar usuario", typeof(postController), ex));
                }
            }

            return View(postVm);
        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var post = _service.GetById(id);

            if (post == null) { return HttpNotFound(); }

            if (post.Enabled) post.Enabled = false;
            else post.Enabled = true;

            post.UpdatedAt = DateTime.Now;

            _service.Update(post);

            //TODO: AGREGAR LA CLASE MESSAGE SESSION
            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Usuario modificado", string.Format("El usuario {0} fue modificado con éxito", post.Name)));

            return RedirectToAction("Index");
        }
    }
}


