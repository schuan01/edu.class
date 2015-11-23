using EduClass.Entities;
using System.Linq;

namespace EduClass.Repository
{
    public interface IMailRepository : IBaseRepository<Mail>
    {
        IQueryable<Mail> GetMailsReceived(Person person);
        IQueryable<Mail> GetMailsSent(Person person);
    }
}
