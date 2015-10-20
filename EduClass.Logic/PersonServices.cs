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

        public Person SignIn(string userName, string password)
        {
            return _personRepository.SignIn(userName, password);
        }

        public Person GetByUserName(string userName)
        {
            return _personRepository.GetByUserName(userName);
        }

        public void ChangePassword(int id, string newpassword)
        {
            var user = _personRepository.GetById(id);

            if (user == null) { throw new ArgumentNullException("User"); }

            user.Password = newpassword;

            _personRepository.Update(user);
            _unitOfWork.Commit();
        }
    }
}
