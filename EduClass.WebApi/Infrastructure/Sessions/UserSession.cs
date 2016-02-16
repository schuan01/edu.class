using EduClass.Entities;
using EduClass.Logic;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace EduClass.WebApi.Infrastructure.Sessions
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

        public static IList<Group> GetUserGroups()
        {
            IList<Group> grupos = new List<Group>();
            
            var _service = DependencyResolver.Current.GetService<IGroupServices>();
            grupos = _service.GetActiveGroups(GetCurrentUser());
            return grupos;
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
                else
                {
                    IList<Group> grupos = new List<Group>();

                    var _service = DependencyResolver.Current.GetService<IGroupServices>();
                    grupos = _service.GetActiveGroups(GetCurrentUser());
                    if (grupos.Count <= 0)
                        return null;//Que devuelva NULL si la persona no pertence a ningun grupo

                    SetCurrentGroup(grupos[0]);//Obtengo el primer grupo que venga y lo coloco en la Session
                    return grupos[0];

                }

            }

            throw new Exception("Error al obtener Grupo");
        }
    }
}