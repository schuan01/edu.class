using EduClass.Entities;
using Mvc.Mailer;
using System.Collections.Generic;

namespace EduClass.Web.Mailers
{
    public interface IUserMailer
    {
        MvcMailMessage Welcome(Person user,string urlPerson);
        MvcMailMessage PasswordReset(string email, string urlReset);
        MvcMailMessage InviteUserToGroup(List<string> mails, Group g, string urlGroup);
    }
}