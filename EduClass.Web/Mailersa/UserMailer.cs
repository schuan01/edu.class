using EduClass.Entities;
using Mvc.Mailer;

namespace EduClass.Web.Mailers
{ 
	public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome(Person user)
		{
			//ViewBag.Data = someObject;
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
	}
}