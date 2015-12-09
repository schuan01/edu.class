using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class AvatarServices : EntityService<Avatar>, IAvatarServices
    {

        IUnitOfWork _unitOfWork;
        IAvatarRepository _avatarRepository;

        public AvatarServices(IUnitOfWork unitOfWork, IAvatarRepository avatarRepository)
            : base(unitOfWork, avatarRepository)
        {
            _unitOfWork = unitOfWork;
            _avatarRepository = avatarRepository;
        }
    }
}
