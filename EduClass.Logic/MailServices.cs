using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class MailServices : EntityService<Mail>, IMailServices
    {
        IUnitOfWork _unitOfWork;
        IMailRepository _mailRepository;

        public MailServices(IUnitOfWork unitOfWork, IMailRepository mailRepository)
            : base(unitOfWork, mailRepository)
        {
            _unitOfWork = unitOfWork;
            _mailRepository = mailRepository;
        }

    }
}
