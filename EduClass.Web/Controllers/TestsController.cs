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
using EduClass.Web.Infrastructure.Mappers;
using EduClass.Web.Infrastructure.Helpers;
using System.Collections.Generic;
using log4net;

namespace EduClass.Web.Controllers
{
	[Authorize]
	public class TestsController : Controller
	{
		private static ITestServices _service;
		private static IResponseServices _response;
		private static IQuestionServices _question;
		private static ICollection<string> _questionTypes;
        private static IPersonServices _person;
        private static ILog _log;

		public TestsController(ITestServices service, IQuestionServices question, IResponseServices response, IPersonServices person, ILog log)
		{
			_service = service;
			_response = response;
			_question = question;
            _person = person;
            _log = log;

			_questionTypes = new List<string>()
			{
				{"tof-"},
				{"redaction-"},
				{"op-"},
				{"chk-"}
			};
		}

		public ActionResult Index()
		{
			if (UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
			
			if (UserSession.GetCurrentGroup() == null) { return View(); }

			var list = _service.GetAll(UserSession.GetCurrentGroup().Id).OrderByDescending(a => a.CreatedAt);

			return View(list);
		}

        public ActionResult MyTests()
        {
            if (UserSession.GetCurrentUser() is Teacher) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var testList = _service.GetTestStudents(UserSession.GetCurrentUser().Id);

            return View(testList);
        }

		// GET: Test
		[HttpGet]
		public ActionResult Create()
		{
            if (UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			var test = new TestViewModel();
			test.GroupId = UserSession.GetCurrentGroup().Id;
			return View(test);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Name, Description, StartDate, EndDate, GroupId")]TestViewModel testVm)
		{
            if (UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			if (ModelState.IsValid)
			{
				try
				{
					//Execute the mapping 
					var test = AutoMapper.Mapper.Map<TestViewModel, Test>(testVm);

					test.GroupId = UserSession.GetCurrentGroup().Id;
					test.CreatedAt = DateTime.Now;
					test.Enabled = false; //Se pone deshabilitada para que no les aparezcan a los alumnos

					if (UserSession.GetCurrentUser() is Teacher)
						_service.Create(test);
					else
						throw new Exception("El usuario actual no es un Profesor");

					MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Creacion Exitosa", "La prueba se creo correctamente. <br />Haz clic en el icono [ <i class=\"fa fa-question\" style=\"font-size: 25px\"></i> ] para agregar preguntas a la prueba."));

					return RedirectToAction("Index");

				}
				catch (Exception ex)
				{
                    _log.Error("Test - Create(Post) -> ", ex);
					MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Error", "No se pudo crear la prueba"));
				}
			}

			return View(testVm);
		}

		[HttpGet]
		public ActionResult Edit(int id = 0)
		{
            if (id == 0 || UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
			var test = AutoMapper.Mapper.Map<Test, TestViewModel>(_service.GetById(id));

			return View(test);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id, Name, Description, StartDate, EndDate, GroupId")]TestViewModel testVm)
		{
            if (UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			if (ModelState.IsValid)
			{
				try
				{
					var entity = _service.GetById(testVm.Id);

					var test = AutoMapper.Mapper.Map<TestViewModel, Test>(testVm, entity);
					test.UpdatedAt = DateTime.Now;

					_service.Update(test);

					MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Test modificado", string.Format("El test {0} fue modificado con éxito", testVm.Name)));

					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
                    _log.Error("Test - Edit(Post) -> ", ex);
					MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "", "Error al modificar test"));
				}
			}

			return View(testVm);
		}

		public ActionResult Disable(int id = 0)
		{
            if (id == 0 || UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			var test = _service.GetById(id);

			if (test == null) { return HttpNotFound(); }

			if (test.Enabled)
			{
				test.Enabled = false;
			}
			else 
			{ 
				test.Enabled = true; 
			}

			test.UpdatedAt = DateTime.Now;

			_service.Update(test);

			MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Test modificado", string.Format("Se ha agregado una alerta a los alumnos para que comienzen el test.", test.Name)));

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult ReadyToTest(int id = 0)
		{ 
			if (UserSession.GetCurrentUser() is Teacher || id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            //var testList = _service.GetEnabledTestForStudents(UserSession.GetCurrentGroup().Id);
            var responseList = _response.GetResponsesByStudent(UserSession.GetCurrentUser().Id);

            if (responseList.Any(x => x.Question.TestId == id))
            {
                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.WARNING, "Importante", "Ya has hecho ésta prueba.,"));
                return RedirectToAction("Index", "Board");
            }
            
			return View(_service.GetById(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ReadyToTest(int id, FormCollection frm)
		{
			if (id == null && id == 0 && UserSession.GetCurrentUser() is Teacher) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
            try
            {
			    var test = _service.GetById(id);

			    foreach (var question in test.Questions)
			    {
				
				    foreach (var item in frm)
				    {
						
					    var qType = _questionTypes.FirstOrDefault(a => item.ToString().Contains(a));

					    if (qType != null)
					    {
						    var questionResponseId = StringHelper.ReturnNumbers(item.ToString());

						    if (question.Id == questionResponseId)
						    {
							    if (qType.Contains("tof-"))
							    {
                                    var response = GetResponse(question);
                                    response.QuestionOption = question.QuestionOptions.FirstOrDefault();

								    var vToF = frm[item.ToString()];

								    if (vToF.Equals("true")) { response.TrueOrFalse = true; }
								    else if (vToF.Equals("false")) { response.TrueOrFalse = false; }
								    else { response.TrueOrFalse = null; }

                                    if (response.QuestionOption.TrueOrFalse == response.TrueOrFalse)
								    {
									    response.IsCorrect = true;
                                    }
                                    else
                                    {
                                        response.IsCorrect = false;
                                    }

                                    _response.Create(response);
								    break;
							    }
							    else if (qType.Contains("redaction-"))
							    {
                                    var response = GetResponse(question);
                                    response.QuestionOption = question.QuestionOptions.FirstOrDefault();

                                    if (response.QuestionOption != null)
								    {
									    response.Content = frm[item.ToString()];

                                        _response.Create(response);

                                        break;
								    }
							    }
							    else if (qType.Contains("op-") || qType.Contains("chk-"))
							    {

                                    var opIds = frm[item.ToString()].Split(',');

                                    foreach (var opId in opIds)
                                    {
								        var qopResponseId = StringHelper.ReturnNumbers(opId);

								        var qop = question.QuestionOptions.FirstOrDefault(q => q.Id == qopResponseId);

								        if (qop != null)
								        {
                                            var response = GetResponse(question);
									        response.QuestionOption = qop;

									        if (qop.IsCorrect != null && qop.IsCorrect == true) { response.IsCorrect = true; }

                                            _response.Create(response);
								        }
                                    }
								
                                    break;
							    }
						    }
					    }
				    }
			    }

                MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Importante", "Tu prueba fué enviada. Cuando termine el periodo podrás visualizar los resultados."));

                return RedirectToAction("Index", "Board");
            }
            catch (Exception ex)
            {
                _log.Error("Test - ReadyToTest(Post) -> ", ex);
                throw;
            }
		}

        [HttpGet]
        public ActionResult ViewStudentsTest(int id = 0)
        {
            if (UserSession.GetCurrentUser() is Student || id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            ViewBag.Test = _service.GetById(id);
            var a = _response.GetStudentsTests(id);

            return View(a);
        }

        [HttpGet]
        public ActionResult ViewResponsesStudent(int idTest = 0, int idStudent = 0)
        {
            if (idTest == 0 || idStudent == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            ViewBag.StudentResponses = _response.GetResponsesByStudent(idStudent).ToList();
            ViewBag.TestQuestions = _service.GetById(idTest).Questions.ToList();
            ViewBag.Student = _person.GetById(idStudent);
            ViewBag.TestId = idTest;

            if (UserSession.GetCurrentUser() is Student) 
            {
                return View("ViewResponses");
            }

            return View();
        }

        [HttpPost]
        public ActionResult MarkRedactionOptionType(int idResponse, bool responseT) 
        {
            if (idResponse == 0 || UserSession.GetCurrentUser() is Student) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var response = _response.GetById(idResponse);

            response.IsCorrect = responseT;

            _response.Update(response);

            return RedirectToAction("ViewResponsesStudent", new { idTest = response.Question.TestId, idStudent = response.StudentId });
        }
        
        
        /*************** METHODS ***************/
        private Response GetResponse(Question question)
        {
            Response response = new Response();

            response.CreatedAt = DateTime.Now;
            response.Question = question;
            response.QuestionId = question.Id;
            response.Student = (Student)_person.GetById(UserSession.GetCurrentUser().Id);

            return response;
        }
	}
}


