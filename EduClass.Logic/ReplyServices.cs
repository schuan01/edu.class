using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class ReplyServices : EntityService<Reply>, IReplyServices
    {
        
        IUnitOfWork _unitOfWork;
        IReplyRepository _replyRepository;

        public ReplyServices(IUnitOfWork unitOfWork, IReplyRepository replyRepository)
            : base(unitOfWork, replyRepository)
        {
            _unitOfWork = unitOfWork;
            _replyRepository = replyRepository;
        }
    }
}
