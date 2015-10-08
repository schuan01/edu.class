using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class PersonServices : EntityService<Person>, IPersonServices
    {
        
        IUnitOfWork _unitOfWork;
        IPersonRepository _personRepository;

        public PersonServices(IUnitOfWork unitOfWork, IPersonRepository personRepository)
            : base(unitOfWork, personRepository)
        {
            _unitOfWork = unitOfWork;
            _personRepository = personRepository;
        }

            public void Create(Person entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Person entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetAll()
        {
            throw new NotImplementedException();
        }

        public Person SignIn(string userName, string password)
        {
            return _personRepository.SignIn(userName, password);
        }

        public void Update(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}
