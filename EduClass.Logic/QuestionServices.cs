using EduClass.Entities;
using EduClass.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Logic
{
    public class QuestionServices: EntityService<Question>, IQuestionServices
    {
        
        IUnitOfWork _unitOfWork;
        IQuestionRepository _questionRepository;

        public QuestionServices(IUnitOfWork unitOfWork, IQuestionRepository questionRepository)
            : base(unitOfWork, questionRepository)
        {
            _unitOfWork = unitOfWork;
            _questionRepository = questionRepository;
        }

        public IQueryable<Question> GetAll(int id) 
        {
            return _questionRepository.GetAll(id);   
        }
    }
}
