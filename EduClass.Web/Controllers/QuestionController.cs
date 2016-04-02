using EduClass.Logic;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EduClass.Web.Infrastructure.Sessions;
using EduClass.Web.Infrastructure.ViewModels;
using EduClass.Entities;
using System.Text.RegularExpressions;
using EduClass.Web.Infrastructure.Helpers;
using log4net;

namespace EduClass.Web.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private static IQuestionServices _service;
        private static ITestServices _testService;
		private static IQuestionOptionServices _questionOptionService;
        private static ILog _log; 

        public QuestionController(IQuestionServices service,  ITestServices testService, IQuestionOptionServices questionOptionService, ILog log)
        {
            _service = service;
            _testService = testService;
			_questionOptionService = questionOptionService;
            _log = log;
        }
        
        public ActionResult Index(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }

            var list = _service.GetAll(id).OrderBy(x => x.Id);
			ViewBag.TestId = id;

            return View(list);
        }

		[HttpGet]
        public ActionResult Create(int idTest = 0) 
        {
            if (idTest == 0) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }

            var test = _testService.GetById(idTest);

            if (UserSession.GetUserGroups().Any(x => x.Id == test.GroupId))
            {
                var question = new QuestionViewModel();
				
				question.TestId = test.Id;
				
				ViewBag.QuestionType = Enum.GetValues(typeof(QuestionType)).Cast<QuestionType>().ToList();

                return View(question);
            }
            else
            {
                _log.Warn("Question - Create -> Not Authorize");
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(QuestionViewModel questionVm, FormCollection frm)
		{
			if (questionVm.TestId == 0) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }

			try
			{
				var test = _testService.GetById(questionVm.TestId);

				if (UserSession.GetUserGroups().Any(x => x.Id == test.GroupId))
				{				

					//Execute the mapping 
					var question = AutoMapper.Mapper.Map<QuestionViewModel, Question>(questionVm);
                    question.QuestionType = (QuestionType)questionVm.QuestionType;
					question.CreatedAt = DateTime.Now;
					question.Enabled = true;

					switch (questionVm.QuestionType)
					{
						case 0: //True Or False

							var opQuestionToF = new QuestionOption();

							opQuestionToF.CreatedAt = DateTime.Now;

							var vChkTrue = frm["chkTrue"];
							var vChkFalse = frm["chkFalse"];
							bool vTrue = false;
							bool vFalse = false;

							if (vChkTrue != null && vChkTrue.Equals("on")) { vTrue = true; }
							if (vChkFalse != null && vChkFalse.Equals("on")) { vFalse = true; }

							//True or False siempre son correctas porque se basan en una row para la validación.
							opQuestionToF.IsCorrect = true;

							if (vTrue && vFalse)
							{
								opQuestionToF.TrueOrFalse = null;
							}
							else
							{
								opQuestionToF.TrueOrFalse = ((vTrue) ? vTrue : false);
							}

							question.QuestionOptions.Add(opQuestionToF);

							break;
						case 1: //Redaction

							var opQuestionR = new QuestionOption();

							opQuestionR.CreatedAt = DateTime.Now;

                            var vResponseCorrect = frm["txtRedaction"];
							string response = String.Empty;

							if (!String.IsNullOrEmpty(vResponseCorrect))
							{
								response = vResponseCorrect.ToString();	
							}

							//Redaction siempre son correctas porque se basan en una row para la validación.
							opQuestionR.IsCorrect = true;
							opQuestionR.Text = response;

							question.QuestionOptions.Add(opQuestionR);

							break;
						case 2: //Options

							//Busco la correcta opción correcta
							int correctId = 0;

							foreach (var i in frm)
							{
								if (i.ToString().Contains("chkOp-"))
								{
                                    correctId = StringHelper.ReturnNumbers(i.ToString());
								}
							}

							foreach (var item in frm)
							{
								var opVId = 0;
								var fieldName = item.ToString();


								if (fieldName.Contains("opTxt"))
								{
									var op = new QuestionOption();
									op.CreatedAt = DateTime.Now;

                                    opVId = StringHelper.ReturnNumbers(fieldName);

									if (frm[fieldName] != null)
									{
										op.Content = frm[fieldName].ToString();

										if (opVId == correctId)
										{
											op.IsCorrect = true;
										}
									}

									question.QuestionOptions.Add(op);
								}

							}
							break;
						case 3: //CHECKS

							foreach (var item in frm)
							{
								var opVId = 0;
								var fieldName = item.ToString();


								if (fieldName.Contains("opTxt"))
								{
									var op = new QuestionOption();
									op.CreatedAt = DateTime.Now;

                                    opVId = StringHelper.ReturnNumbers(fieldName);

									if (frm[fieldName] != null)
									{
										op.Content = frm[fieldName].ToString();

										foreach (var i in frm)
										{
											if (i.ToString().Contains("chkOp-"))
											{
												if (StringHelper.ReturnNumbers(i.ToString()) == opVId)
												{
													op.IsCorrect = true;	
												}
											}
										} 
									}

									question.QuestionOptions.Add(op);
								}

							}
							break;
						default:
                            throw new Exception("No existe");
							break;
					}

					_service.Create(question);

					return RedirectToAction("Index", new { id = questionVm.TestId });
				}
				else
				{
                    _log.Warn("Question - Create -> Not Authorize");
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
				}

			}
			catch (Exception ex)
			{
                _log.Error("Question - Create -> ", ex);
				throw;
			}

			return RedirectToAction("Create", new { idTest = questionVm.TestId });

		}

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }

            var question = _service.GetById(id);
            var questionVm = AutoMapper.Mapper.Map<Question, QuestionViewModel>(question);

            ViewBag.QuestionOptions = question.QuestionOptions;

            if (UserSession.GetUserGroups().Any(x => x.Id == question.Test.GroupId))
            {
                ViewBag.QuestionType = Enum.GetValues(typeof(QuestionType)).Cast<QuestionType>().ToList();

                return View(questionVm);
            }
            else
            {
                _log.Warn("Question - Edit -> Not Authorize");
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionViewModel questionVm, FormCollection frm)
        {
            if (questionVm.TestId == 0 || questionVm.Id == 0) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }

            try
            {
                var entity = _service.GetById(questionVm.Id);

                if (entity != null && UserSession.GetUserGroups().Any(x => x.Id == entity.Test.GroupId))
                {

                    var question = AutoMapper.Mapper.Map<QuestionViewModel, Question>(questionVm, entity);
                    question.UpdatedAt = DateTime.Now;

                    switch (question.QuestionType)
                    {
                        case QuestionType.TRUEORFALSE: //True Or False

                            var vOptChkId = frm["chkToFId"];
                            int tofId;

                            if (vOptChkId != null && Int32.TryParse(vOptChkId, out tofId))
                            {
                                var opQuestionToF = _questionOptionService.GetById(tofId);
                                
                                var vChkTrue = frm["chkTrue"];
                                var vChkFalse = frm["chkFalse"];
                                bool vTrue = false;
                                bool vFalse = false;

                                if (vChkTrue != null && vChkTrue.Equals("on")) { vTrue = true; }
                                if (vChkFalse != null && vChkFalse.Equals("on")) { vFalse = true; }

                                //True or False siempre son correctas porque se basan en una row para la validación.
                                opQuestionToF.IsCorrect = true;

                                if (vTrue && vFalse)
                                {
                                    opQuestionToF.TrueOrFalse = null;
                                }
                                else
                                {
                                    opQuestionToF.TrueOrFalse = ((vTrue) ? vTrue : false);
                                }

                                opQuestionToF.UpdatedAt = DateTime.Now;

                                _questionOptionService.Update(opQuestionToF);
                            }

                            break;
                        case QuestionType.REDACTION: //Redaction

                            var vOptRedaction = frm["txtRedactionId"];
                            int redactionId;

                            if (vOptRedaction != null && Int32.TryParse(vOptRedaction, out redactionId))
                            {
                                var opQuestionR = _questionOptionService.GetById(redactionId);

                                var vResponseCorrect = frm["txtRedaction"];
                                string response = String.Empty;

                                if (!String.IsNullOrEmpty(vResponseCorrect))
                                {
                                    response = vResponseCorrect.ToString();
                                }

                                //Redaction siempre son correctas porque se basan en una row para la validación.
                                opQuestionR.IsCorrect = true;
                                opQuestionR.Text = response;

                                opQuestionR.UpdatedAt = DateTime.Now;

                                _questionOptionService.Update(opQuestionR);
                            }
                            break;
                        case QuestionType.OPTIONS: //Options

                            //Busco la correcta opción correcta
                            int correctId = 0;

                            foreach (var i in frm)
                            {
                                if (i.ToString().Contains("chkOp-"))
                                {

                                    correctId = StringHelper.ReturnNumbers(i.ToString());
                                }
                            }

                            foreach (var item in frm)
                            {
                                var opVId = 0;
                                var fieldName = item.ToString();

                                if (fieldName.Contains("opTxt"))
                                {

                                    if (frm[fieldName] != null)
                                    {

                                        if (fieldName.Contains("new"))
                                        {
                                            var op = new QuestionOption();
                                            op.CreatedAt = DateTime.Now;

                                            if (frm[fieldName] != null)
                                            {
                                                op.Content = frm[fieldName].ToString();

                                                var correct = frm["chkOp-new-" + StringHelper.ReturnNumbers(fieldName)];

                                                if (correct != null)
                                                {
                                                    foreach (var j in question.QuestionOptions)
                                                    {
                                                        j.IsCorrect = false;
                                                    }

                                                    op.IsCorrect = true;
                                                }
                                            }

                                            question.QuestionOptions.Add(op);
                                        }
                                        else
                                        {

                                            opVId = StringHelper.ReturnNumbers(fieldName);

                                            var qop = question.QuestionOptions.FirstOrDefault(i => i.Id == opVId);

                                            if (qop != null)
                                            {
                                                qop.UpdatedAt = DateTime.Now;
                                                qop.Content = frm[fieldName].ToString();

                                                if (opVId == correctId)
                                                {
                                                    foreach (var j in question.QuestionOptions)
                                                    {
                                                        j.IsCorrect = false;
                                                    }

                                                    qop.IsCorrect = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case QuestionType.CHECKS: //CHECKS

                            foreach (var item in frm)
                            {
                                var opVId = 0;
                                var fieldName = item.ToString();

                                if (fieldName.Contains("opTxt"))
                                {

                                    if (frm[fieldName] != null)
                                    {

                                        if (fieldName.Contains("new"))
                                        {
                                            var op = new QuestionOption();
                                            op.CreatedAt = DateTime.Now;

                                            if (frm[fieldName] != null)
                                            {
                                                op.Content = frm[fieldName].ToString();

                                                var correct = frm["chkOp-new-" + StringHelper.ReturnNumbers(fieldName)];

                                                if (correct != null)
                                                {
                                                    op.IsCorrect = true;
                                                }
                                                else
                                                {
                                                    op.IsCorrect = false;
                                                }
                                            }

                                            question.QuestionOptions.Add(op);
                                        }
                                        else
                                        {

                                            opVId = StringHelper.ReturnNumbers(fieldName);

                                            var qop = question.QuestionOptions.FirstOrDefault(i => i.Id == opVId);

                                            if (qop != null)
                                            {
                                                qop.UpdatedAt = DateTime.Now;
                                                qop.Content = frm[fieldName].ToString();

                                                var correct = frm["chkOp-" + opVId];

                                                if (correct != null)
                                                {
                                                    qop.IsCorrect = true;
                                                }
                                                else
                                                {
                                                    qop.IsCorrect = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    _service.Update(question);

                    return RedirectToAction("Index", new { id = questionVm.TestId });
                }
                else
                {
                    _log.Warn("Question - Edit(Post) -> Not Authorize");
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }
            catch (Exception ex)
            {
                _log.Warn("Question - Edit(Post) -> ", ex);

                throw;
            }

            return RedirectToAction("Edit", new { idTest = questionVm.TestId });

        }

        public ActionResult Disable(int id = 0)
        {
            if (id == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var question = _service.GetById(id);

            if (question == null) { return HttpNotFound(); }

            if (question.Enabled)
            {
                question.Enabled = false;
            }
            else
            {
                question.Enabled = true;
            }

            question.UpdatedAt = DateTime.Now;

            _service.Update(question);

            MessageSession.SetMessage(new MessageHelper(Enum_MessageType.SUCCESS, "Question eliminada", ""));

            return RedirectToAction("Index", new { id = question.TestId });
        }

        [HttpPost]
        public ActionResult RemoveOption(int idop = 0) 
        {
            if (idop == 0) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }

            var option = _questionOptionService.GetById(idop);

            var idQuestion = option.QuestionId;

            _questionOptionService.Delete(idop);

            return RedirectToAction("Edit", new { id = idQuestion });
        }
    }
}