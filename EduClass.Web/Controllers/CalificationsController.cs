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
    public class CalificationsController : Controller
    {

        private static ICalificationServices _service;
        private static IPersonServices _personService;
        private static IGroupServices _groupService;
        private ILog _log;

        public CalificationsController(ICalificationServices service, IPersonServices personService, IGroupServices groupService, ILog log)
        {
            _service = service;
            _personService = personService;
            _groupService = groupService;
            _log = log;
        }



        public ActionResult Index()
        {
            if (UserSession.GetCurrentUser() is Student)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "No Autorizado", "No puede acceder a esta página"));
                _log.Error("Califications - Index => No Autorizado");
                return RedirectToAction("Index", "Board");
            }

            IEnumerable<Person> miembros = new List<Person>();
            if (UserSession.GetCurrentGroup() != null)
            {
                //LOS ALUMNOS DEL GRUPO ACTUAL
                miembros = _groupService.GetById(UserSession.GetCurrentGroup().Id).Students;

            }
            else
            {
                
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No hay un grupo seleccionado."));
                _log.Error("Califications - Index => No hay grupo seleccioado");
                return RedirectToAction("Index", "Board");
            }
            return View(miembros);
        }

        [HttpGet]
        public ActionResult Save()
        {
            return View(new CalificationViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(FormCollection formCollection)
        {

            try
            {
                string idOral = "";
                string idEscrito = "";
                string idOtro = "";
                String valorOral = "";
                String valorEscrito = "";
                String valorOtro = "";

                IEnumerable<Person> miembros = _groupService.GetById(UserSession.GetCurrentGroup().Id).Students;
                IEnumerable<Calification> notasAlumno = new List<Calification>();
               
                foreach (Person m in miembros)
                {
                    idOral = "oral" + m.Id;
                    idEscrito = "escrito" + m.Id;
                    idOtro = "otro" + m.Id;

                    valorOral = formCollection[idOral];
                    valorEscrito = formCollection[idEscrito];
                    valorOtro = formCollection[idOtro];

                    if ((valorEscrito != null && valorEscrito != "")&& (valorOral != null && valorOral != "") && (valorOtro != null && valorOtro != ""))
                    {
                        //Si ya tiene una calificacion, la buscamos y la editamos
                        notasAlumno = ((Student)m).Califications;
                        Calification c = notasAlumno.FirstOrDefault(x => x.GroupId == UserSession.GetCurrentGroup().Id);//Deberia ser una unica nota por alumno del grupo
                        if (c != null)
                        {
                            c.Oral = Convert.ToInt32(valorOral);
                            c.Test = Convert.ToInt32(valorEscrito);
                            c.Other = Convert.ToInt32(valorOtro);
                            c.Average = (c.Oral + c.Test + c.Other) / 3;//Calculo promedio
                            c.UpdatedAt = DateTime.Now;
                            _service.Update(c);
                        }
                        else//Si es la primera vez
                        {

                            Calification ca = new Calification();
                            ca.Oral = Convert.ToInt32(valorOral);
                            ca.Test = Convert.ToInt32(valorEscrito);
                            ca.Other = Convert.ToInt32(valorOtro);
                            ca.Average = (ca.Oral + ca.Test + ca.Other) / 3;//Calculo promedio
                            ca.CreatedAt = DateTime.Now;
                            ca.GroupId = UserSession.GetCurrentGroup().Id;
                            ca.StudentId = m.Id;
                            _service.Create(ca);
                        }
                    }

                }

                if (miembros.Count() == 0)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No hay alumnos en el grupo seleccionado"));
                    _log.Error("Califications - Save => No hay alumnos en el grupo seleccionado");
                }
                else
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Modificacion Exitosa", "Las calificaciones se modificaron correctamente"));
                }

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo guardar las calificaciones"));
                _log.Error("Califications - Save", ex);
            }
            return RedirectToAction("Index");
        }
    }
}