using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class FaqController : Controller
    {
        [HttpGet]
        public ActionResult ViewFaq()
        {
            return View();
        }
    }
}