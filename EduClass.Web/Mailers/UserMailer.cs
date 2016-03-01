using EduClass.Entities;
using Mvc.Mailer;
using System.Collections.Generic;

namespace EduClass.Web.Mailers
{ 
	public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome(Person user, string urlPerson)
		{
            ViewBag.urlPerson = urlPerson;

            return Populate(x =>
			{
				x.Subject = "Bienvenido " + user.FirstName;
				x.ViewName = "Welcome";
				x.To.Add(user.Email);
			});
		}
 
		public virtual MvcMailMessage PasswordReset(string email, string urlReset)
		{
            ViewBag.UrlReset = urlReset;

			return Populate(x =>
			{
				x.Subject = "Reseteo de contraseña";
				x.ViewName = "PasswordReset";
				x.To.Add(email);
			});
		}

        public virtual MvcMailMessage InviteUserToGroup(List<string> mails, Group g, string urlGroup)
        {
            ViewBag.UrlGroup = urlGroup;

            return Populate(x =>
            {
                x.Subject = "Usted ha sido invitado al grupo " + g.Name;
                x.ViewName = "InvitedToGroup";
                foreach (string valor in mails)
                {
                    x.To.Add(valor);
                }
            });
        }
    }
}