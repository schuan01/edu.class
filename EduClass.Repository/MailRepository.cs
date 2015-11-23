using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class MailRepository : BaseRepository<Mail>, IMailRepository
    {
        public MailRepository(DbContext context) : base(context)
        {

        }

        public IQueryable<Mail> GetMailsReceived(Person person)
        {
            //GetAll().Where(u => u.PersonsTo.Any(s => s.Id == person.Id)).ToList();
            var algo = GetAll().Where(x => x.PersonsTo.Any(s => s.Id == person.Id));
            return algo;
        }

        public IQueryable<Mail> GetMailsSent(Person person)
        {
            
            return GetAll().Where(x => x.PersonFromId == person.Id);
        }

        /*public Person SignIn(string userName, string password)
        {
            return dbSet.Where(x => x.UserName == userName && x.Password == password && x.Enabled).FirstOrDefault();
        }

        public Person GetByUserName(string userName)
        {
            return dbSet.Where(x => x.UserName == userName).FirstOrDefault();
        }*/
    }
}
