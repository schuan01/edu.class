using EduClass.Entities;
using EduClass.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Logic
{
	public class QuestionOptionServices: EntityService<QuestionOption>, IQuestionOptionServices
	{
		
		IUnitOfWork _unitOfWork;
		IQuestionOptionRepository _questionOptionRepository;

		public QuestionOptionServices(IUnitOfWork unitOfWork, IQuestionOptionRepository questionOptionRepository)
			: base(unitOfWork, questionOptionRepository)
		{
			_unitOfWork = unitOfWork;
			_questionOptionRepository = questionOptionRepository;
		}
	}
}
