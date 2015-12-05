using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private IPersonServices _person;
        private IGroupServices _group;

        public BoardController(IPersonServices person, IGroupServices group)
        {
            _person = person;
            _group = group;
        }
        // GET: Board
        public ActionResult Index(int id = 0)
        {
            IList<Post> postList = new List<Post>();


            if (id == 0){

                var groups = _group.GetActiveGroups(UserSession.GetCurrentUser());
                
                if (groups.Count() != 0)
	            {
                    foreach (var item in groups)
                    {
                        postList.Union(item.Posts);
                    }
	            }
            }
            else
            {
                var group = _group.GetGroupByIdWithPosts(id);

                if (group != null)
                {
                    postList.Union(group.Posts);
                }
            }

            return View(postList.OrderByDescending(i => i.CreatedAt));
        }
    }
}