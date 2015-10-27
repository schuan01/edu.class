using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class PostServices : EntityService<Post>, IPostServices
    {
        
        IUnitOfWork _unitOfWork;
        IPostRepository _postRepository;

        public PostServices(IUnitOfWork unitOfWork, IPostRepository personRepository)
            : base(unitOfWork, personRepository)
        {
            _unitOfWork = unitOfWork;
            _postRepository = personRepository;
        }
    }
}
