using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class GroupServices : EntityService<Group>, IGroupServices
    {
        
        IUnitOfWork _unitOfWork;
        IGroupRepository _groupRepository;

        public GroupServices(IUnitOfWork unitOfWork, IGroupRepository groupRepository)
            : base(unitOfWork, groupRepository)
        {
            _unitOfWork = unitOfWork;
            _groupRepository = groupRepository;
        }

        public Group GetByKey(string key)
        {
            return _groupRepository.GetByKey(key);
        }

        public IList<Group> GetActiveGroups(Person person)
        {
            if (person is Teacher)
            {
                return _groupRepository.GetActiveGroupsByTeacher(person.Id);
            }
            else if (person is Student)
            {
                return _groupRepository.GetActiveGroupsByStudent(person.Id);
            }

            throw new InvalidCastException();
        }

        public Group GetGroupByIdWithPosts(int id)
        {
            return _groupRepository.GetGroupByIdWithPosts(id);
        }

        public void DetachStudent(int id, int studentId)
        {
            _groupRepository.DetachStudent(id, studentId);
        }
    }
}
