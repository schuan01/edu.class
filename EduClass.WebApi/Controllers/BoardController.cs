﻿using EduClass.Entities;
using EduClass.Logic;
using EduClass.WebApi.Infrastructure.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EduClass.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Board")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BoardController : ApiController
    {
        private static IGroupServices _service;
        private static IPersonServices _personService;
        private static IPostServices _postService;
        private static IReplyServices _replyService;
        public BoardController(IGroupServices service, IPersonServices personService, IPostServices postService, IReplyServices replyService)
        {
            _service = service;
            _personService = personService;
            _postService = postService;
            _replyService = replyService;


        }

        [HttpGet]
        [Route("GetPostsGroup")]
        [AllowAnonymous]
        public IHttpActionResult GetPostsGroup(int id)
        {
            List<Post> postsSencillo = new List<Post>();
            List<File> archivos = new List<File>();
            List<Reply> respuestas = new List<Reply>();
            try
            {
                Group g = _service.GetById(id);

                foreach (Post p in g.Posts)
                {
                    Post po = new Post();
                    po.Id = p.Id;
                    po.Title = p.Title;
                    po.Content = p.Content;
                    po.CreatedAt = p.CreatedAt;
                    po.Person = new Person();
                    po.Person.Avatar = new Avatar();
                    po.Person.UserName = p.Person.UserName;
                    po.Person.FirstName = p.Person.FirstName;
                    po.Person.LastName = p.Person.LastName;
                    po.Person.Avatar.UrlPhoto = p.Person.Avatar.UrlPhoto;
                    foreach (Reply r in p.Replays)
                    {
                        Reply re = new Reply();
                        re.Id = r.Id;
                        re.Person = new Person();
                        re.Person.Avatar = new Avatar();
                        re.Person.FirstName = r.Person.FirstName;
                        re.Person.LastName = r.Person.LastName;
                        re.Person.Avatar.UrlPhoto = r.Person.Avatar.UrlPhoto;
                        re.Content = r.Content;
                        re.CreatedAt = r.CreatedAt;
                        respuestas.Add(re);
                    }
                    foreach (File f in p.Files)
                    {
                        File arch = new File();
                        arch.Id = f.Id;
                        arch.Name = f.Name;
                        arch.UrlFile = f.UrlFile;
                        archivos.Add(arch);
                    }
                    po.Files = archivos;
                    po.Replays = respuestas;
                    postsSencillo.Add(po);
                    archivos = new List<File>();
                    respuestas = new List<Reply>();


                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "Ocurrio un error en la solicitud" });
            }


            return Json(JsonConvert.SerializeObject(postsSencillo));
        }

        [HttpPost]
        [Route("Post")]
        [AllowAnonymous]
        public IHttpActionResult Post(PostViewModel postVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Person p = _personService.GetById(postVm.PersonId);
                    if (p is Student && p.Silenced)
                        return Json(new { error = "No puedes crear Post cuando estas silenciado, contacte al Profesor del grupo" });

                    if (postVm.GroupId == 0)
                        return Json(new { error = "No puedes crear Post si no hay grupo seleccionado" });

                    var post = AutoMapper.Mapper.Map<PostViewModel, Post>(postVm);


                    post.CreatedAt = DateTime.Now;
                    post.Enabled = true;

                    _postService.Create(post);

                    return Json(new { mensaje = "Se creo correcamente el Post" });
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {

                    return Json(new { error = "Error al crear Post en la BD" });
                }
                catch (Exception ex)
                {

                    return Json(new { error = "Error al crear post, por favor contacte con el Administrador." });

                }
            }

            return Json(new { error = "Error al crear el Post" });
        }

        [HttpPost]
        [Route("Reply")]
        [AllowAnonymous]
        public IHttpActionResult Reply([FromBody]JObject parametros)
        {
            var usuario = parametros["personId"].ToObject<int>();
            var idPost = parametros["postId"].ToObject<int>();
            var contenido = parametros["content"].ToObject<string>();
            

            if (ModelState.IsValid)
            {
                if (idPost == 0 || String.IsNullOrEmpty(contenido))
                {
                    return Json(new { error = "Error al crear comentario" });
                }

                try
                {

                    Person p = _personService.GetById(usuario);
                    if (!p.Silenced)
                    {

                        var reply = new Reply();
                        reply.Content = HttpUtility.HtmlEncode(contenido);
                        reply.CreatedAt = DateTime.Now;
                        reply.PersonId = usuario;
                        reply.PostId = idPost;
                        reply.Enabled = true;

                        _replyService.Create(reply);

                        return Json(new { mensaje = "Se creo correcamente el Reply" });
                    }
                    else
                    {
                        return Json(new { error = "No puedes comentar un Post si estas silenciado" });
                        //_log.Warn("Board - Reply => No authorize");
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException du)
                {
                    //_log.Error("Board - Reply -> ", du);
                    throw;
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error al crear un Reply" });
                    //_log.Error("Board - Reply", ex);
                }
            }

            return Json(new { error = "Error al crear un Reply" });
        }
    }
}
