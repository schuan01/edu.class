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
        private IFileServices _file;

        public BoardController(IPersonServices person, IGroupServices group, IPostServices post, IReplyServices reply, IFileServices file)
        {
            _person = person;
            _group = group;
            _post = post;
            _reply = reply;
            _file = file;
        }

        // GET: Board
        public ActionResult Index(int id = 0)
        { 

            IList<Post> postList = new List<Post>();

            try
            {
                if (id == 0)
                {

                    if (UserSession.GetCurrentGroup() != null)
                    {
                        var group = _group.GetGroupByIdWithPosts(UserSession.GetCurrentGroup().Id);

                        if (group != null)
                        {
                            postList = group.Posts.ToList();
                        }
                    }
                }


                ViewBag.PostTypeList = Enum.GetValues(new PostType().GetType());
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se puede abrir el Board"));
            }


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
                    Person p = _person.GetById(UserSession.GetCurrentUser().Id);
                    if (p is Student && p.Silenced)
                        throw new Exception("No puedes crear Post cuando estas silenciado, contacte al Profesor del grupo");

                    if(UserSession.GetCurrentUser() == null)
                        throw new Exception("No puedes crear Post si no hay grupo seleccionado");

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

                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear Post en la BD"));

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear post, por favor contacte con el Administrador."));
                }
            }

            return RedirectToAction("Index");
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

                    Person p = _person.GetById(UserSession.GetCurrentUser().Id);
                    if (!p.Silenced)
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
                    else
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "No Autorizado", "No puedes comentar un Post cuando estas silenciado, contacte al Profesor del grupo"));

                    }
                    

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

            return RedirectToAction("Index");
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
                //Solo el creador del Post o el Profesor del grupo puede eliminar el Post
                if (post.PersonId == UserSession.GetCurrentUser().Id || post.Group.Teacher.Id == UserSession.GetCurrentUser().Id)
                {

                    var replycant = post.Replays.Count();
                    var replys = post.Replays.ToArray();

                    for (int i = 0; i < replycant; i++)
                    {
                        var reply = replys[i];
                        _reply.Delete(reply.Id);
                    }


                    //PRIMERO SACO TODOS LOS ID
                    List<int> archivos = new List<int>();
                    foreach (File f in post.Files)
                    {
                        archivos.Add(f.Id);
                    }

                    //LUEGO BORRO TODOS
                    foreach (int i in archivos)
                    {
                        post.Files.Remove(post.Files.First(x => x.Id == i));
                    }
                        
                    _post.Delete(post.Id);
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Completado", "El post fue borrado con éxito"));
                }
                else
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "No Autorizado", "No tiene permisos para borrar el Post"));
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException du)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar Post en la BD"));
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar post, por favor contacte con el Administrador."));
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveReply(int id = 0)
        {
            if (id == 0)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al eliminar el reply", "Reply id no existe"));
                return RedirectToAction("Index");
            }

            try
            {
                //Solo el creador del Reply o del Post o el Profesor del grupo puede eliminar el Reply
                Reply r = _reply.GetById(id);
                if (r.PersonId == UserSession.GetCurrentUser().Id || r.Post.PersonId == UserSession.GetCurrentUser().Id || r.Post.Group.Teacher.Id == UserSession.GetCurrentUser().Id)
                {
                    _reply.Delete(r);
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Completado", "El reply fue borrado con éxito"));
                }
                else
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "No Autorizado", "No tiene permisos para borrar el comentario"));

                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar reply, por favor contacte con el Administrador."));
            }
            return RedirectToAction("Index");
        }
    }
}