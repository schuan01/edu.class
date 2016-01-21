using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;


namespace EduClass.Web.Controllers
{
    public class FilesLibraryController : Controller
    {

        private static IFileServices _service;
        private static IPersonServices _servicePerson;
        private string carpetaUsuario;

        public FilesLibraryController(IFileServices service, IPersonServices personService)
        {
            _service = service;
            _servicePerson = personService;
            carpetaUsuario = "UsersFolders\\" + UserSession.GetCurrentUser().UserName;//Inicia el controlador y setea la carpeta

        }


        // GET: FilesLibrary
        public ActionResult Index()
        {
            List<Entities.File> archivos = new List<Entities.File>();
            //Obtengo los archivos que publico el Person
            Person p = _servicePerson.GetById(UserSession.GetCurrentUser().Id);
            archivos = p.Files.ToList();

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
                        f.UrlFile = "~\\" + carpetaUsuario + "\\" + file.FileName;
                        f.Name = file.FileName;
                        f.Person = p;
                        f.CreatedAt = DateTime.Now;

                        _service.Create(f);

                        UserSession.SetCurrentUser(p);//Actualizo la Session



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

            }

            return null;


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int fileId)
        {
            try
            {
                Entities.File f = _service.GetById(fileId);
                if (f != null)
                {
                    string fullPath = Request.MapPath(f.UrlFile);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        _service.Delete(f);
                        return RedirectToAction("Index", "FilesLibrary");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al borrar el archivo."));

            }
            return null;


        }
    }
}