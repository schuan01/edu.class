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
    public class BoardController : Controller
    {
        private IPersonServices _person;
        private IGroupServices _group;
        private IPostServices _post;
        private IReplyServices _reply;

        public BoardController(IPersonServices person, IGroupServices group, IPostServices post, IReplyServices reply)
        {
            _person = person;
            _group = group;
            _post = post;
            _reply = reply;
        }

        // GET: Board
        public ActionResult Index(int id = 0)
        {
            IList<Post> postList = new List<Post>();


            if (id == 0){

                var groups = _group.GetActiveGroups(UserSession.GetCurrentUser());
                
                if (groups.Count() != 0)
	            {
                    //TODO: VER forma de union
                    foreach (var item in groups)
                    {
                        postList.Union(item.Posts);
                    }
	            }
            }
            else
            {
                var group = _group.GetGroupByIdWithPosts(id);

                if (group != null)
                {
                    postList = group.Posts.ToList();
                }
            }

            ViewBag.PostTypeList = Enum.GetValues(new PostType().GetType());

            return View(postList.OrderByDescending(i => i.CreatedAt));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post([Bind(Include = "Title, Content, PostType")]PostViewModel postVm)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var post = AutoMapper.Mapper.Map<PostViewModel, Post>(postVm);

                    post.CreatedAt = DateTime.Now;
                    post.Enabled = true;
                    post.PersonId = UserSession.GetCurrentUser().Id;
                    post.GroupId = UserSession.GetCurrentGroup().Id;

                    //TODO FILES
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        if (file != null)
                        {

                        }
                    }


                    _post.Create(post);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Post creado", "El post fue creado con éxito"));

                    return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear post, por favor contacte con el Administrador."));
                }
            }

            return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(int postId = 0, string content = "")
        {
            if (ModelState.IsValid)
            {
                if (postId == 0 || String.IsNullOrEmpty(content))
                {
                    return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
                }

                try
                {

                    var reply = new Reply();
                    reply.Content = Server.HtmlEncode(content);
                    reply.CreatedAt = DateTime.Now;
                    reply.PersonId = UserSession.GetCurrentUser().Id;
                    reply.PostId = postId;
                    reply.Enabled = true;

                    _reply.Create(reply);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Reply creado", "El reply fue creado con éxito"));

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear post, por favor contacte con el Administrador."));
                }
            }

            return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
        }

        public ActionResult RemovePost(int id = 0)
        {
            if (id == 0)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al eliminar el post", "Post id no existe"));
                return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
            }

            try
            {
                var post = _post.GetById(id);

                if (post.PersonId == UserSession.GetCurrentUser().Id)
                {

                    var replycant = post.Replays.Count();
                    var replys = post.Replays.ToArray();
                    
                    for (int i = 0; i < replycant; i++)
                    {
                        var reply = replys[i];
                        _reply.Delete(reply.Id);
                    }

                    _post.Delete(post.Id);
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Completado", "El post fue borrado con éxito"));
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException du)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar post, por favor contacte con el Administrador."));
            }

            return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
        }

        public ActionResult RemoveReply(int id = 0)
        {
            if (id == 0)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al eliminar el reply", "Reply id no existe"));
                return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
            }

            try
            {

                _reply.Delete(_reply.GetById(id));

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Completado", "El reply fue borrado con éxito"));

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar reply, por favor contacte con el Administrador."));
            }
            return RedirectToAction("Index", new { id = UserSession.GetCurrentGroup().Id });
        }
    }
}