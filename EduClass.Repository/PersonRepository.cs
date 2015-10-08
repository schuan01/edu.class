using System.Linq;
using System.Data.Entity;
using EduClass.Entities;

namespace EduClass.Repository
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(DbContext context) : base(context)
        { }

        public Person SignIn(string userName, string password)
        {
            return _dbset.Where(x => x.UserName.Contains(userName) && x.Password.Contains(password) && x.Enabled).FirstOrDefault();
        }
    }
}
