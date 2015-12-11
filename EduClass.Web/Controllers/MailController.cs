using EduClass.Logic;
using EduClass.Web.Infrastructure.Modules;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using EduClass.Web.Infrastructure;
using EduClass.Web.Infrastructure.ViewModels;
using EduClass.Entities;
using System.Web;

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
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail()
        {
            ViewBag.FromEmail = UserSession.GetCurrentUser().Email;
            ViewBag.PersonsTo = new SelectList(_personService.GetAll().Where(g => g.Enabled == true && g.Id != UserSession.GetCurrentUser().Id).ToList(), "Id", "FirstName");

            return View(new MailViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail([Bind(Include = "Subject, Description, PersonIdTo, PersonEmailFrom")]MailViewModel mailVm)
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
            ViewBag.PersonsTo = new SelectList(_personService.GetAll().Where(g => g.Enabled == true && g.Id != UserSession.GetCurrentUser().Id).ToList(), "Id", "FirstName");

            return View(mailVm);
        }

        //TODO
        [HttpPost]
        public ActionResult DeleteMail(int id)
        {
            return View();
        }
    }
}