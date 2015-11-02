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
