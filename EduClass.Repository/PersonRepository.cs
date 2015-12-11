using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(DbContext context) : base(context)
        { }

        public Person SignIn(string userName, string password)
        {
            return dbSet.Where(x => x.UserName == userName && x.Password == password && x.Enabled)
                .Include(x => x.Avatar)
                .FirstOrDefault();
        }

        public Person GetByUserName(string userName)
        {
            return dbSet.Where(x => x.UserName == userName)
                .Include(x => x.Avatar)
                .FirstOrDefault();
        }

        public void SaveKeyResetPassword(string email, string key)
        {
            throw new NotImplementedException();
        }

        
    }
}
