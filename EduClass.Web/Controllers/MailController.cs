using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Web.Mvc;
using System.Linq;
using EduClass.Web.Infrastructure.ViewModels;
using EduClass.Entities;
using System.Web;
using System.Collections.Generic;
using log4net;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class MailController : Controller
    {
        private static IMailServices _service;
        private static IPersonServices _personService;
        private static IGroupServices _groupService;
        private ILog _log;

        public MailController(IMailServices service, IPersonServices personService, IGroupServices groupService, ILog log)
        {
            _service = service;
            _personService = personService;
            _groupService = groupService;
            _log = log;
        }


        public ActionResult Index(string type)
        {
            
            var list = _service.GetMailsReceived(UserSession.GetCurrentUser()).OrderByDescending(a => a.CreateAt);

            if (type == "Enviados")
            {
                list = _service.GetMailsSent(UserSession.GetCurrentUser()).OrderByDescending(a => a.CreateAt);//Obtengo enviados
                
                    
            }
            else if(type == "Borrados")
            {
                list = _service.GetMailsDeleted(UserSession.GetCurrentUser()).OrderByDescending(a => a.CreateAt);//Obtengo los borrados
            }

            foreach (var v in list)
            {
                v.Description = HttpUtility.HtmlDecode(v.Description);
            }

            
            return View(list);
        }

      
        [HttpGet]
        public ActionResult SendEmail()
        {
            try
            {
                if (UserSession.GetCurrentGroup() != null)
                {
                    if (!_personService.GetById(UserSession.GetCurrentUser().Id).Silenced)
                    {
                        List<Person> personas = new List<Person>();
                        //El remitente. simplemente para mostrar
                        ViewBag.FromUser = UserSession.GetCurrentUser().FirstName + UserSession.GetCurrentUser().LastName;

                        //Obtengo los miembros del grupo actual
                        IEnumerable<Person> miembros = _groupService.GetById(UserSession.GetCurrentGroup().Id).Students;
                        foreach (Person m in miembros)
                        {
                            personas.Add(m);
                        }

                        if (UserSession.GetCurrentUser() is Student)
                        {
                            //Ademas de los miembros, agrego al Teacher
                            personas.Add(_groupService.GetById(UserSession.GetCurrentGroup().Id).Teacher);
                        }

                        miembros = personas;


                        //Filtro los miembros
                        //Solo los activos
                        //Ademas no tengo que ser yo
                        ViewBag.PersonsTo = new SelectList(miembros.Where(g => g.Enabled == true && g.Id != UserSession.GetCurrentUser().Id).ToList()
                           .Select(s => new
                           {
                               Id = s.Id,
                               NombreCompleto = s.FirstName + " " + s.LastName
                           })
                       , "Id", "NombreCompleto");
                    }
                    else
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "No Autorizado", "No puedes mandar un Mensaje cuando estas silenciado, contacte al Profesor del grupo"));
                        _log.Error("Mail - SendEmail => Alumno Silenciado");
                        return RedirectToAction("Index", "Mail");
                    }
                }
                else
                {
                    ViewBag.FromUser = "";
                    ViewBag.PersonsTo = new SelectList("VACIO");
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No hay un grupo seleccionado."));
                    _log.Error("Mail - SendEmail => No hay grupo seleccionado");
                    return RedirectToAction("Index", "Mail");
                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error abrir nuevo Mensaje."));
                _log.Error("Mail - SendEmail", ex);
                return RedirectToAction("Index", "Mail");
            }

            return View(new MailViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail([Bind(Include = "Subject, Description, PersonIdTo")]MailViewModel mailVm)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    Person p = _personService.GetById(UserSession.GetCurrentUser().Id);
                    if (!p.Silenced)
                    {


                        //Execute the mapping 
                        var mail = AutoMapper.Mapper.Map<MailViewModel, Mail>(mailVm);

                        mail.PersonFromId = UserSession.GetCurrentUser().Id;
                        mail.CreateAt = DateTime.Now;
                        mail.Subject = "(" + UserSession.GetCurrentGroup().Name + ") - " + mail.Subject;
                        mail.ReadAt = null;
                        mail.Enabled = true;


                        foreach (int i in mailVm.PersonIdTo)
                        {
                            Person personTo = _personService.GetById(i);
                            if (personTo.Enabled && personTo != null)
                            {
                                mail.PersonsTo.Add(personTo);

                            }
                        }

                        mail.Description = HttpUtility.HtmlEncode(mail.Description);
                        _service.Create(mail);
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Envio Exitoso", "El Email fue enviado correctamente"));
                        
                    }
                    else
                    {
                        MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "No Autorizado", "No puedes mandar un Mensaje cuando estas silenciado, contacte al Profesor del grupo"));
                        _log.Error("Mail - SendEmail => Alumno Silenciado");
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                    _log.Error("Mail - SendEmail", ex);
                }
            }

            return RedirectToAction("Index", "Mail");
        }


        [HttpPost]
        public ActionResult ReadMail(int id)
        {

            try
            {
                Mail m = _service.GetById(id);
                Person p = _personService.GetById(UserSession.GetCurrentUser().Id);

                if (p.MailsRecieved.Any(x => x.Id == m.Id))//Siempre y cuando el mail este en recibidos
                {
                    m.ReadAt = DateTime.Now;
                    _service.Update(m);
                }

            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al leer el correo."));
                _log.Error("Mail - ReadMail", ex);

            }

            return RedirectToAction("Index", "Mail");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ReplyMail(int valorId, string Mensaje)
        {


            try
            {
                if (valorId != 0)
                {
                    Mail mailAnterior = _service.GetById(valorId);
                    Mail mailNuevo = new Mail();
                    mailNuevo.Subject = mailAnterior.Subject;//Mismo Asunto
                    Mensaje = Mensaje + "<br><hr><p><b>Mensaje anterior de " + mailAnterior.PersonFrom.FirstName +" "+ mailAnterior.PersonFrom.LastName +"</b></p>" + HttpUtility.HtmlDecode(mailAnterior.Description);
                    mailNuevo.Description = HttpUtility.HtmlEncode(Mensaje);
                    mailNuevo.PersonFromId = UserSession.GetCurrentUser().Id;
                    mailNuevo.CreateAt = DateTime.Now;
                    mailNuevo.ReadAt = null;
                    mailNuevo.Enabled = true;
                    mailNuevo.PersonsTo.Add(mailAnterior.PersonFrom);//Solo al remitente

                    _service.Create(mailNuevo);
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Envio Exitoso", "El Email fue enviado correctamente"));
                }
                else
                {
                    throw new Exception("No se ha seleccionado ningun mensaje");
                }



            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al enviar el correo."));
                _log.Error("Mail - ReplyEmail", ex);
            }

            return RedirectToAction("Index", "Mail");
        }

        //BORRA DEFINITIVO RECIBIDOS
        //SOLO BORRA DE LA COLECCION PERTINETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMail(int mailId)
        {
            try
            {
                if (mailId != 0)
                {
                    Mail m = _service.GetById(mailId);
                    
                    Person p = _personService.GetById(UserSession.GetCurrentUser().Id);

                    if (p.MailsRecieved.Any(x => x.Id == m.Id))
                    {
                        p.MailsRecieved.Remove(p.MailsRecieved.First(x => x.Id == m.Id));
                        _personService.Update(p);
                    }
                    //NO PODES BORRAR ALGO ENVIADO POR LA RELACION 1 - N.
                    /*else if (p.MailsSends.Any(x => x.Id == m.Id))
                    {
                        p.MailsSends.Remove(p.MailsSends.First(x => x.Id == m.Id));
                        _personService.Update(p);
                    }*/
                    else
                    {
                        throw new Exception("El mail no pertence a tus Enviados/Recibidos");
                    }

                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Borrado Exitoso", "El mail fue borrado correctamente"));

                }
                else
                {
                    throw new Exception("Error al borrar el mail");
                }
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", ex.Message));
                _log.Error("Mail - DeleteMail", ex);
            }

            return RedirectToAction("Index", "Mail");
        }
    }
}