using EduClass.Entities;
using System.Collections.Generic;

namespace EduClass.Logic
{
    public interface IMailServices : IEntityService<Mail>
    {
        IEnumerable<Mail> GetMailsReceived(Person person);
        IEnumerable<Mail> GetMailsSent(Person person);
    }
}
