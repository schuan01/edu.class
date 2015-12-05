using EduClass.Entities;
using Mvc.Mailer;

namespace EduClass.Web.Mailers
{
    public interface IUserMailer
    {
        MvcMailMessage Welcome(Person user);
        MvcMailMessage PasswordReset(string email, string urlReset);
    }
}