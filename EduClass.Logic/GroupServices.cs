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

       
    }
}
