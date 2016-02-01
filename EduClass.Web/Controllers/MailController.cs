using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Web.Mvc;
using System.Linq;
using EduClass.Web.Infrastructure.ViewModels;
using EduClass.Entities;
using System.Web;
using System.Collections.Generic;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class MailController : Controller
    {
        private static IMailServices _service;
        private static IPersonServices _personService;

        public MailController(IMailServices service, IPersonServices personService)
        {
            _service = service;
            _personService = personService;
        }


        public ActionResult Index(string type)
        {
            
            var list = _service.GetMailsReceived(UserSession.GetCurrentUser()).OrderByDescending(a => a.CreateAt);

            if(type == "Enviados")
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

        // GET: Mail
        [HttpGet]
        public ActionResult SendEmail()
        {
            try
            {
                ViewBag.FromUser = UserSession.GetCurrentUser().FirstName + UserSession.GetCurrentUser().LastName;

                //ViewBag.PersonsTo = new SelectList(_personService.GetAll().Where(g => g.Enabled == true && g.Id != UserSession.GetCurrentUser().Id && (UserSession.GetCurrentGroup().Students.Any(x => x.Id == g.Id) || UserSession.GetCurrentGroup().Teacher.Id == g.Id)).ToList()
                ViewBag.PersonsTo = new SelectList(_personService.GetAll().Where(g => g.Enabled == true && g.Id != UserSession.GetCurrentUser().Id).ToList()
                    .Select(s => new
                    {
                        Id = s.Id,
                        NombreCompleto = s.FirstName + " " + s.LastName
                    })
                , "Id", "NombreCompleto");
            }
            catch (Exception ex)
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error abrir SendMail."));
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
                    //Execute the mapping 
                    var mail = AutoMapper.Mapper.Map<MailViewModel, Mail>(mailVm);

                    mail.PersonFromId = UserSession.GetCurrentUser().Id;
                    mail.CreateAt = DateTime.Now;
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
                    return RedirectToAction("Index", "Mail");
                    

                }
                catch (Exception ex)
                {
                    MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al enviar el correo."));
                }
            }


            //Vuelvo a cargar la lista para que no de exception(si aplica)
            ViewBag.PersonsTo = new SelectList(_personService.GetAll().Where(g => g.Enabled == true && g.Id != UserSession.GetCurrentUser().Id).ToList()
                .Select(s => new
                {
                    Id = s.Id,
                    NombreCompleto = s.FirstName + " " + s.LastName
                })
            , "Id", "NombreCompleto");

            return View(mailVm);
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
                    mailNuevo.Subject = mailAnterior.Subject;
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
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "Error al borrar  el correo."));
            }

            return RedirectToAction("Index", "Mail");
        }
    }
}