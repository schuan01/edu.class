using Dropbox.Api;
using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using Ionic.Zip;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;


namespace EduClass.Web.Controllers
{
    public class FilesLibraryController : Controller
    {

        private static IFileServices _service;
        private static IPersonServices _servicePerson;
        private static IPostServices _servicePost;
        private string carpetaUsuario;
        private ILog _log;
        private string App_key = "";
        private string App_secret = "";

        
        

        public FilesLibraryController(IFileServices service, IPersonServices personService, IPostServices postService, ILog log)
        {
            _service = service;
            _servicePerson = personService;
            _servicePost = postService;
            carpetaUsuario = "UsersFolders\\" + UserSession.GetCurrentUser().UserName;//Inicia el controlador y setea la carpeta
            _log = log;

            App_key = WebConfigurationManager.AppSettings["DropboxAppKey"];
            App_secret = WebConfigurationManager.AppSettings["DropboxAppSecret"];

        }

        static async Task Run()
        {
            using (var dbx = new DropboxClient("Z413XdG4OvAAAAAAAAAACt-98b9GqkzqtyIap78Ac5vNBO4gB10VTHlf2fIBKZaB"))
            {

                var full = await dbx.Users.GetCurrentAccountAsync();
                Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
            }
        }

        // GET: FilesLibrary
        public ActionResult Index(string tipo)
        {
            //var task = Task.Run((Func<Task>)Run);
            //task.Wait();

            IOrderedEnumerable<Entities.File> archivos = null;
            //Obtengo los archivos que publico el Person
            Person p = _servicePerson.GetById(UserSession.GetCurrentUser().Id);
            if (p is Teacher)
            {
                archivos = p.Files.ToList().OrderByDescending(x => x.CreatedAt);

                if (tipo == "imagenes")
                    archivos = archivos.Where(x => MimeMapping.GetMimeMapping(x.Name).Contains("image")).ToList().OrderByDescending(x => x.CreatedAt);

                if (tipo == "audio")

                    archivos = archivos.Where(x => MimeMapping.GetMimeMapping(x.Name).Contains("audio")).ToList().OrderByDescending(x => x.CreatedAt);

                if (tipo == "documentos")
                    archivos = archivos.Where(x => MimeMapping.GetMimeMapping(x.Name).Contains("office")).ToList().OrderByDescending(x => x.CreatedAt);

                if (tipo == "pdf")
                    archivos = archivos.Where(x => MimeMapping.GetMimeMapping(x.Name).Contains("application/pdf")).ToList().OrderByDescending(x => x.CreatedAt);

            }
            else
            {
                //Lleva  a Board si no sos Alumno
                _log.Error("FilesLibrary - Index => El usuario actual no es Profesor");
                return RedirectToAction("Board","Index");

            }

            return View(archivos);
        }

        [Authorize]
        public ActionResult Connect()
        {
            var redirect = DropboxOAuth2Helper.GetAuthorizeUri(
                OAuthResponseType.Code,
                App_key,
                RedirectUri,
                "");
                //RedirectUri,
                //this.currentUser.ConnectState);

            return Redirect(redirect.ToString());
        }

        private string RedirectUri
        {
            get
            {
                if (this.Request.Url.Host.ToLowerInvariant() == "localhost")
                {
                    return "http://localhost:55555/FilesLibrary/Index";
                }

                var builder = new UriBuilder(
                    Uri.UriSchemeHttps,
                    this.Request.Url.Host);

                builder.Path = "/FilesLibrary/Auth";

                return builder.ToString();
            }
        }

        // GET: /FilesLibrary/Auth
        [Authorize]
        public async Task<ActionResult> AuthAsync(string code, string state)
        {
            try
            {
                /*if (this.currentUser.ConnectState != state)
                {
                    this.Flash("There was an error connecting to Dropbox.");
                    return this.RedirectToAction("Index");
                }*/

                var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                    code,
                    App_key,
                    App_secret,
                    this.RedirectUri);

                /*this.currentUser.DropboxAccessToken = response.AccessToken;
                this.currentUser.ConnectState = string.Empty;
                await this.store.SaveChangesAsync();

                this.Flash("This account has been connected to Dropbox.", FlashLevel.Success);*/
                //return RedirectToAction("Profile");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", e.Message));
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadFile(int id = 0)
        {
            HttpPostedFileBase file = null;
            bool isSavedSuccessfully = true;
            string fName = "";
            string path = "";
            try
            {

                if (!(UserSession.GetCurrentUser() is Teacher))
                {
                    throw new Exception("El usuario actual no es un Profesor");
                }
                foreach (string fileName in Request.Files)
                {
                    file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        //La carpeta es el userName del usuario
                        var originalDirectory = new DirectoryInfo(string.Format("{0}" + carpetaUsuario + "\\", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "FileLibrary");

                        var fileName1 = Path.GetFileName(file.FileName);


                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        path = string.Format("{0}\\{1}", pathString, file.FileName);

                        file.SaveAs(path);//Termina el guardado del archivo fisico


                        //Despues de guardar el archivo
                        Person p = UserSession.GetCurrentUser();//Traigo el usuario actual
                        p = _servicePerson.GetById(p.Id);

                        //Y creo el nuevo archivo
                        Entities.File f = new Entities.File();
                        f.UrlFile = "~\\" + carpetaUsuario + "\\FileLibrary\\" + file.FileName;
                        f.Name = file.FileName;
                        f.Person = p;
                        f.CreatedAt = DateTime.Now;

                        _service.Create(f);





                    }

                }


            }
            catch (Exception ex)
            {
                //Borro el archivo fisico si se llega a crear
                if (file != null)
                {
                    FileInfo fi1 = new FileInfo(path);
                    fi1.Delete();
                }

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al subir el archivo."));
                _log.Error("FilesLibrary - UploadFile", ex);

        }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error al guardar el archivo" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult ImportFromDropBox(int fileId)
        {

           

            try
            {
                Entities.File f = _service.GetById(fileId);
                if (f != null)
                {

                    return File(Request.MapPath(f.UrlFile), MediaTypeNames.Application.Octet, f.Name);
                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al descargar el archivo."));

            }

            return null;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult DownloadFile(int fileId)
        {
            try
            {
                Entities.File f = _service.GetById(fileId);
                if (f != null)
                {

                    return File(Request.MapPath(f.UrlFile), MediaTypeNames.Application.Octet, f.Name);
                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al descargar el archivo."));
                _log.Error("FilesLibrary - DownloadFile", ex);

            }

            return null;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult DownloadMoreFiles(FormCollection formCollection)
        {
            try
            {
                String valores = formCollection["seleccionadosDescargar"];
                List<string> identificadores = valores.Split(',').ToList<string>();
                identificadores.Reverse();

                var outputStream = new MemoryStream();

                using (var zip = new ZipFile())
                {
                    foreach (string arch in identificadores)
                    {
                        if (arch != "")
                        {
                            Entities.File f = _service.GetById(Convert.ToInt32(arch));
                            if (f != null)
                            {
                                string fullPath = Server.MapPath(f.UrlFile);
                                zip.AddFile(fullPath,"");

                            }
                        }
                    }

                    zip.Save(outputStream);
                    outputStream.Position = 0;
                }

                
                return File(outputStream, "application/zip", "ArchivosEduClass.zip");

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al descargar el archivo."));
                _log.Error("FilesLibrary - DownloadMoreFiles", ex);

            }

            return null;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int fileId)
        {
            try
            {
                List<int> idFiles = new List<int>();
                List<int> idPosts = new List<int>();
                List<int> idReplys = new List<int>();
                Entities.File f = _service.GetById(fileId);
                if (f != null && f.PersonId == UserSession.GetCurrentUser().Id)
                {
                    string fullPath = Server.MapPath(f.UrlFile);
                    if (System.IO.File.Exists(fullPath))
                    {
                        

                        //BORRO ARCHIVOS DE LOS POSTS
                        foreach (var a in f.Posts)
                        {
                            foreach (var b in a.Files)
                            {
                                idFiles.Add(b.Id);
                            }

                            foreach (int i in idFiles)
                            {
                                a.Files.Remove(a.Files.FirstOrDefault(x => x.Id == i));
                            }

                            idPosts.Add(a.Id);
                        }

                        //BORRO LOS POSTS
                        foreach (int i in idPosts)
                        {
                            Post p = _servicePost.GetById(i);
                            foreach (var reply in p.Replays)
                            {
                                idReplys.Add(reply.Id);
                            }
                            foreach (int a in idReplys)
                            {
                                p.Replays.Remove(p.Replays.FirstOrDefault(x => x.Id == a));
                            }
                            f.Posts.Remove(f.Posts.FirstOrDefault(x => x.Id == i));
                            _servicePost.Delete(_servicePost.GetById(i));
                           
                        }

                        //BORRO LA IMAGEN DEFINITIVAMENTE
                        _service.Delete(f);

                        //Borro fisicamente
                        System.IO.File.Delete(fullPath);

                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Archivo", "Archivo borrado con éxito."));

                    }
                    else
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Archivo", "El archivo no existe."));
                        _log.Error("FilesLibrary - DeleteFile => File not Found");
                    }

                }
                else
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Archivo", "El archivo no existe."));
                    _log.Error("FilesLibrary - DeleteFile => File not Found");
                }
            }
            catch(Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al borrar el archivo."));
                _log.Error("FilesLibrary - DeleteFile", ex);

            }
            return RedirectToAction("Index", "FilesLibrary");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShareMoreFiles(FormCollection formCollection)
        {
            try
            {
                String valores = formCollection["seleccionados"];
                List<string> identificadores = valores.Split(',').ToList<string>();
                identificadores.Reverse();

                Person p = _servicePerson.GetById(UserSession.GetCurrentUser().Id);
                if (p is Student && p.Silenced)
                    throw new Exception("No puedes compartir un archivo cuando estas silenciado, contacte al Profesor del grupo");

                Group g = UserSession.GetCurrentGroup();
                if (g == null)
                    throw new Exception("No hay grupo seleccionado");

                Post post = new Post();
                post.Title = "Nuevos archivos compartidos";
                post.Content = "Chequea los nuevos archivos que he compartido con ustedes";
                post.GroupId = UserSession.GetCurrentGroup().Id;
                post.PersonId = UserSession.GetCurrentUser().Id;
                post.PostType = PostType.Material;
                post.CreatedAt = DateTime.Now;
                post.Enabled = true;

                
                foreach (string valor in identificadores)
                {
                    if (valor != "")
                    {
                        Entities.File f = _service.GetById(Convert.ToInt32(valor));
                        if (f != null && f.PersonId == UserSession.GetCurrentUser().Id)//Solo puedo compartir archivos mios
                        {
                            post.Files.Add(f);
                        }
                    }
                }

                if (post.Files.Count > 0)
                {
                    _servicePost.Create(post);
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Post creado", "Has compartido correctamente los archivos"));
                }
                else
                {
                    throw new Exception("No hay archivos seleccionados para compartir");
                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("FilesLibrary - ShareMoreFiles", ex);

            }
            return RedirectToAction("Index", "FilesLibrary");


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMoreFiles(FormCollection formCollection)
        {
            try
            {

                String valores = formCollection["seleccionadosBorrar"];
                List<string> identificadores = valores.Split(',').ToList<string>();
                identificadores.Reverse();

                try
                {
                    foreach (string valor in identificadores)
                    {
                        if (valor != "")
                        {
                            List<int> idFiles = new List<int>();
                            List<int> idPosts = new List<int>();
                            List<int> idReplys = new List<int>();
                            Entities.File f = _service.GetById(Convert.ToInt32(valor));

                            if (f != null && f.PersonId == UserSession.GetCurrentUser().Id)//Solo puedo borrar archivos mios
                            {
                                string fullPath = Server.MapPath(f.UrlFile);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    
                                    //BORRO ARCHIVOS DE LOS POSTS
                                    foreach (var a in f.Posts)
                                    {
                                        foreach (var b in a.Files)
                                        {
                                            idFiles.Add(b.Id);
                                        }

                                        foreach (int i in idFiles)
                                        {
                                            a.Files.Remove(a.Files.FirstOrDefault(x => x.Id == i));
                                        }

                                        idPosts.Add(a.Id);
                                    }

                                    //BORRO LOS POSTS
                                    foreach (int i in idPosts)
                                    {
                                        Post p = _servicePost.GetById(i);
                                        foreach (var reply in p.Replays)
                                        {
                                            idReplys.Add(reply.Id);
                                        }
                                        foreach (int a in idReplys)
                                        {
                                            p.Replays.Remove(p.Replays.FirstOrDefault(x => x.Id == a));
                                        }
                                        f.Posts.Remove(f.Posts.FirstOrDefault(x => x.Id == i));
                                        _servicePost.Delete(_servicePost.GetById(i));
                                    }

                                    //BORRO LA IMAGEN DEFINITIVAMENTE
                                    _service.Delete(f);
                                    //Borro fisicamente
                                    System.IO.File.Delete(fullPath);

                                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Archivo", "Archivo borrado con éxito."));

                                }
                                else
                                {
                                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Archivo", "El archivo no existe."));
                                    _log.Error("FilesLibrary - DeleteMoreFiles => File not Found");
                                }

                            }
                            else
                            {
                                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Archivo", "El archivo no existe."));
                                _log.Error("FilesLibrary - DeleteMoreFiles => File not Found");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al borrar el archivo."));
                    _log.Error("FilesLibrary - DeleteMoreFiles",ex);

                }

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("FilesLibrary - DeleteMoreFiles", ex);

            }
            return RedirectToAction("Index", "FilesLibrary");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShareFile(int fileId)
        {
            try
            {
                Person p = _servicePerson.GetById(UserSession.GetCurrentUser().Id);
                if (p is Student && p.Silenced)
                    throw new Exception("No puedes compartir un archivo cuando estas silenciado, contacte al Profesor del grupo");

                Group g = UserSession.GetCurrentGroup();
                if(g == null)
                    throw new Exception("No hay grupo seleccionado");

                Entities.File f = _service.GetById(fileId);
                if (f != null)
                {
                    Post post = new Post();
                    post.Title = "Nuevos archivos compartidos";
                    post.Content = "Chequea los nuevos archivos que he compartido con ustedes";
                    post.GroupId = UserSession.GetCurrentGroup().Id;
                    post.PersonId = UserSession.GetCurrentUser().Id;
                    post.PostType = PostType.Material;//Por defecto es Material
                    if (System.Web.MimeMapping.GetMimeMapping(f.Name).Contains("image"))
                    {
                        post.PostType = PostType.Image;
                    }  
                    post.Files.Add(f);
                    post.CreatedAt = DateTime.Now;
                    post.Enabled = true;

                    _servicePost.Create(post);

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Post creado", "Has compartido correctamente los archivos"));


                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("FilesLibrary - ShareFile", ex);

            }
            return RedirectToAction("Index", "FilesLibrary");


        }
    }
}