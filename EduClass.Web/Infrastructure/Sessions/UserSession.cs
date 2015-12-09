using EduClass.Entities;
using EduClass.Logic;
using System;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Infrastructure.Sessions
{
    public class UserSession
    {
        private static string sessionName = "educlass_session";
        private static string sessionNameGroup = "educlass_session_group";

        public static Person GetCurrentUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Session[sessionName] != null)
                {
                    return (Person)HttpContext.Current.Session[sessionName];
                }

                var _service = DependencyResolver.Current.GetService<IPersonServices>();

                var userName = HttpContext.Current.User.Identity.Name;

                SetCurrentUser(_service.GetByUserName(userName));

                return GetCurrentUser();
            }

            throw new Exception("Error to get user");
        }

        public static void SetCurrentUser(Person user)
        {
            HttpContext.Current.Session[sessionName] = user;
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static void SetCurrentGroup(Group group)
        {
            HttpContext.Current.Session[sessionNameGroup] = group;
        }

        public static Group GetCurrentGroup()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Session[sessionNameGroup] != null)
                {
                    return (Group)HttpContext.Current.Session[sessionNameGroup];
                }

            }

            throw new Exception("Error to get group");
        }
    }
}