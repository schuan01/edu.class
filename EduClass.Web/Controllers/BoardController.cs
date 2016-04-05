using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using EduClass.Web.Infrastructure.ViewModels;
using log4net;
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
        private ITestServices _test;
        private IResponseServices _response;
		private ILog _log;

        public BoardController(IPersonServices person, IGroupServices group, IPostServices post, IReplyServices reply, IFileServices file, ITestServices test, IResponseServices response, ILog log)
        {
            _person = person;
            _group = group;
            _post = post;
            _reply = reply;
            _file = file;
            _test = test;
            _response = response;
             
			_log = log;
        }

        // GET: Board
        public ActionResult Index(int id = 0)
        { 

            IList<Post> postList = new List<Post>();

            try
            {
                if (UserSession.GetUserGroups().Count() != 0)
                { 
                    var group = _group.GetGroupByIdWithPosts(UserSession.GetCurrentGroup().Id);

                    if (group != null)
                    {
                        postList = group.Posts.ToList();
                    }
                }
                else
                {
                    return View();
                }

                if (UserSession.GetCurrentUser() is Student)
                {
                    var testList = _test.GetEnabledTestForStudents(UserSession.GetCurrentGroup().Id);
                    var responseList = _response.GetResponsesByStudent(UserSession.GetCurrentUser().Id);

                    var resolved = from t in testList
                                   join r in responseList on t.Id equals r.Question.TestId into resultTest
                                   from tr in resultTest.DefaultIfEmpty()
                                   where tr == null
                                   select t;


                    if (resolved.Count() > 0) { ViewBag.GetTests = resolved; }
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

                    if(UserSession.GetCurrentGroup() == null)
                        throw new Exception("No puedes crear Post si no hay grupo seleccionado");

					if (String.IsNullOrEmpty(postVm.Title) || String.IsNullOrEmpty(postVm.Content))
						{throw new Exception("El titulo o el contenido no pueden estar vacios");}

                    var post = AutoMapper.Mapper.Map<PostViewModel, Post>(postVm);
                    

                    post.CreatedAt = DateTime.Now;
                    post.Enabled = true;
                    post.PersonId = UserSession.GetCurrentUser().Id;
                    post.GroupId = UserSession.GetCurrentGroup().Id;

                    _post.Create(post);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Post creado", "El post fue creado con éxito"));

                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear Post en la BD"));
					_log.Error("Board - Post -> ", du);

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
					_log.Error("Board - Post -> ", ex);
				}
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RePost(int id = 0)
        {

            if (id != 0)
            {
                try
                {
                    Post post = _post.GetById(id);
                    Post nuevoPost = new Post();
                    if (post != null)
                    {
                        Person p = _person.GetById(UserSession.GetCurrentUser().Id);
                        if (p is Student && p.Silenced)
                            throw new Exception("No puedes crear Post cuando estas silenciado, contacte al Profesor del grupo");

                        if (UserSession.GetCurrentGroup() == null)
                            throw new Exception("No puedes crear Post si no hay grupo seleccionado");

                        if (String.IsNullOrEmpty(post.Title) || String.IsNullOrEmpty(post.Content))
                        { throw new Exception("El titulo o el contenido no pueden estar vacios"); }

                        nuevoPost.Title = post.Title;
                        nuevoPost.Content = post.Content;
                        nuevoPost.CreatedAt = DateTime.Now;
                        nuevoPost.Enabled = true;
                        nuevoPost.PersonId = post.PersonId;//El id es el mismo porq solo la misma persona puede hacer Repost
                        nuevoPost.GroupId = post.GroupId;
                        foreach (File f in post.Files)
                        {
                            if (f != null)
                            {
                                Entities.File nuevoFile = _file.GetById(f.Id);
                                if (nuevoFile != null && nuevoFile.PersonId == nuevoPost.PersonId)//Solo puedo compartir archivos mios
                                {
                                    nuevoPost.Files.Add(f);
                                }
                            }
                        }

                        _post.Create(nuevoPost);

                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "RePost creado", "El repost fue creado con éxito"));

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception("El Post seleccionado no existe");
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear Post en la BD"));
                    _log.Error("Board - Post -> ", du);

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                    _log.Error("Board - Post -> ", ex);
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
						_log.Warn("Board - Reply => No authorize");
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
					_log.Error("Board - Reply -> ", du);
                    throw;
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al crear Reply, por favor contacte con el Administrador."));
					_log.Error("Board - Reply", ex);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemovePost(int id = 0)
        {
            if (id == 0)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al eliminar el post", "Post id no existe"));

				_log.Warn("Board - RemovePost -> Post not exists");

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
					_log.Warn("Board - RemovePost -> Not Authorize");
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException du)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar Post en la BD"));
				_log.Error("Board - RemovePost -> ", du);
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar post, por favor contacte con el Administrador."));
				_log.Error("Board - RemovePost -> ", ex);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveReply(int id = 0)
        {
            if (id == 0)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error al eliminar el reply", "Reply id no existe"));

				_log.Warn("Board - RemoveReply -> Post not exists");

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
				_log.Warn("Board - RemoveReply -> ", ex);
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al eliminar reply, por favor contacte con el Administrador."));
            }
            return RedirectToAction("Index");
        }
    }
}