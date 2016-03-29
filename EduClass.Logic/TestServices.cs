using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class TestServices : EntityService<Test>, ITestServices
    {
        
        IUnitOfWork _unitOfWork;
        ITestRepository _testRepository;

        public TestServices(IUnitOfWork unitOfWork, ITestRepository testRepository)
            : base(unitOfWork, testRepository)
        {
            _unitOfWork = unitOfWork;
            _testRepository = testRepository;
        }

        public IEnumerable<Test> GetAll(int id) 
        {
            return _testRepository.GetAll(id);
        }

        public IEnumerable<Test> GetEnabledTestForStudents(int groupId) 
        {
            return _testRepository.GetEnabledTestForStudents(groupId);
        }
    }
}
