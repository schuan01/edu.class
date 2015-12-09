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

        public IEnumerable<Mail> GetMailsReceived(Person person)
        {
            return _mailRepository.GetMailsReceived(person);
        }

        public IEnumerable<Mail> GetMailsSent(Person person)
        {
            return _mailRepository.GetMailsSent(person);
        }

        public IEnumerable<Mail> GetMailsDeleted(Person person)
        {
            return _mailRepository.GetMailsDeleted(person);
        }

    }
}
