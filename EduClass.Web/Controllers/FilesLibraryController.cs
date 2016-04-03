using DropNet;
using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using Ionic.Zip;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private DropNetClient _client;
        
        public FilesLibraryController(IFileServices service, IPersonServices personService, IPostServices postService, ILog log)
        {
            _service = service;
            _servicePerson = personService;
            _servicePost = postService;
            carpetaUsuario = "UsersFolders\\" + UserSession.GetCurrentUser().UserName;//Inicia el controlador y setea la carpeta
            _log = log;
            

            App_key = WebConfigurationManager.AppSettings["DropboxAppKey"];
            App_secret = WebConfigurationManager.AppSettings["DropboxAppSecret"];
            _client = new DropNetClient(App_key, App_secret);

        }

        private string initDropBox()
        {


            Session["DropNetUserLogin"] = _client.GetToken();
            string indexServer = System.Web.HttpContext.Current.Request.Url.ToString();
            indexServer = indexServer.Replace("Connect", "Index");
            return _client.BuildAuthorizeUrl(indexServer);
            
            

        }

        [HttpGet]
        public ActionResult Connect()
        {

            try
            {
                return Redirect(initDropBox());
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al conectar a Dropbox."));
                _log.Error("FilesLibrary - Connect", ex);
            }
            return RedirectToAction("Index");


        }

        // GET: FilesLibrary
        public ActionResult Index(string tipo, string oauth_token)
        {
            IOrderedEnumerable<Entities.File> archivos = new List<Entities.File>().OrderBy(x => x.Id);
            try
            {
                //SOLO APLICA A TEACHER
                if (UserSession.GetCurrentUser() is Teacher)
                {
                    if (oauth_token != null)
                    {
                        if (Session["DropNetUserLogin"] != null)
                        {
                            _client.UserLogin = Session["DropNetUserLogin"] as DropNet.Models.UserLogin;
                            var accessToken = _client.GetAccessToken();

                            string carpeta = "/Aplicaciones/EduClassFolder";
                            var metaData = _client.GetMetaData(carpeta, null, false, false); //Folder
                            var carpetaRaiz = _client.GetMetaData("/Aplicaciones", null, false, false);

                            if (!(carpetaRaiz.Contents.Any(c => c.Is_Dir && c.Path.Contains("EduClassFolder"))))
                            {
                                //Creamos si no existe
                                var folder = _client.CreateFolder(carpeta);
                                metaData = _client.GetMetaData(carpeta, null, false, false); //Folder
                                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Carpeta Creada", "Se creo la carpeta EduClassFolder en /Aplicaciones, vuelva a importar para obtener todos los archivos."));
                                return RedirectToAction("Index");
                            }

                            //La carpeta es el userName del usuario
                            var originalDirectory = new DirectoryInfo(string.Format("{0}" + carpetaUsuario + "\\", Server.MapPath(@"\")));
                            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "FileLibrary");
                            byte[] archivo = null;

                            bool isExists = System.IO.Directory.Exists(pathString);

                            if (!isExists)
                                System.IO.Directory.CreateDirectory(pathString);

                            string rutaArchActual = "";
                            foreach (DropNet.Models.MetaData arch in metaData.Contents)
                            {
                                rutaArchActual = string.Format("{0}\\{1}", pathString, arch.Name);
                                archivo = _client.GetFile("/Aplicaciones/EduClassFolder/" + arch.Name);
                                if (archivo != null)
                                {
                                    Person pe = UserSession.GetCurrentUser();//Traigo el usuario actual
                                    pe = _servicePerson.GetById(pe.Id);

                                    if (!pe.Files.Any(x => x.Name == arch.Name))
                                    {
                                        //Crar en el disco, en la carpeta del usuario
                                        System.IO.File.WriteAllBytes(rutaArchActual, archivo);


                                        //Y creo el nuevo archivo
                                        Entities.File f = new Entities.File();
                                        f.UrlFile = "~\\" + carpetaUsuario + "\\FileLibrary\\" + arch.Name;
                                        f.Name = arch.Name;
                                        f.Person = pe;
                                        f.CreatedAt = DateTime.Now;

                                        _service.Create(f);
                                    }
                                    else
                                    {
                                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.WARNING, "Atención", "Uno o mas archivos ya existian en EduClass, se importaron los nuevos"));
                                    }
                                }
                                else
                                {
                                    _log.Error("FilesLibrary - Index => El byte de archivo " + rutaArchActual + " no existe");
                                }
                            }

                            Session["DropNetUserLogin"] = null;
                        }
                    }
                }
                else
                {
                    //Lleva  a Board si no sos Alumno
                    _log.Error("FilesLibrary - Index => El usuario actual no es Profesor");
                    return RedirectToAction("Board", "Index");
                }
                
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
                    return RedirectToAction("Board", "Index");

                }

                
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al abrir Index."));
                _log.Error("FilesLibrary - Index",ex);
                Session["DropNetUserLogin"] = null;
            }

            return View(archivos);
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

                        Person p = UserSession.GetCurrentUser();//Traigo el usuario actual
                        p = _servicePerson.GetById(p.Id);

                        if (!p.Files.Any(x => x.Name == file.FileName))
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

                            //Y creo el nuevo archivo
                            Entities.File f = new Entities.File();
                            f.UrlFile = "~\\" + carpetaUsuario + "\\FileLibrary\\" + file.FileName;
                            f.Name = file.FileName;
                            f.Person = p;
                            f.CreatedAt = DateTime.Now;

                            _service.Create(f);
                            
                        }
                        else
                        {
                            MessageSession.SetMessage(new MessageHelper(Enum_MessageType.WARNING, "Atención", "Uno o mas archivos ya existian en EduClass, se subieron los nuevos"));
                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //Borro el archivo fisico si se llega a crear
                isSavedSuccessfully = false;
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