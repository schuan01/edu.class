using EduClass.Entities;
using EduClass.Logic;
using EduClass.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPersonServices _service;

        public HomeController(IPersonServices service)
        {
            _service = service;
        }

        // GET: Home
        public ActionResult Index()
        {
            var person = new Person() {
                UserName = "Bruno",
                Email ="brunofranco43@gmail.com",
                FirstName = "Bruno",
                LastName = "Franco",
                CreatedAt = DateTime.Now,
                Birthday = Convert.ToDateTime("08/12/1987"),
                Enabled = true,
                Password = Security.EncodePassword("Password01"),
                IdentificationCard = "HOLA HOLA HOLA"
            };

            _service.Create(person);

            return View();
        }
    }
}