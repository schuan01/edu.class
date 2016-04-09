using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;
using System.Linq;

namespace EduClass.Logic
{
    public class ResponseServices : EntityService<Response>, IResponseServices
    {

        IUnitOfWork _unitOfWork;
        IResponseRepository _responseRepository;

        public ResponseServices(IUnitOfWork unitOfWork, IResponseRepository responseRepository)
            : base(unitOfWork, responseRepository)
        {
            _unitOfWork = unitOfWork;
            _responseRepository = responseRepository;
        }

        public IEnumerable<Response> GetResponsesByStudent(int idStudent)
        {
            return _responseRepository.GetResponsesByStudent(idStudent);
        }

        public IEnumerable<Response> GetResponsesByStudent(int idStudent, int idTest) 
        {
            return _responseRepository.GetResponsesByStudent(idStudent, idTest);
        }

        public IList<Student> GetStudentsTests(int idTest) 
        {
            return _responseRepository.GetStudentsTests(idTest);
        }

        public int GetCorrectResponses(int idStudent, int idTest) 
        {
            return _responseRepository.GetCorrectResponses(idStudent, idTest);
        }
    }
}
