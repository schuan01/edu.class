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

        public IList<Student> GetStudentsTests(int idTest) 
        {
            return _responseRepository.GetStudentsTests(idTest);
        }
    }
}
