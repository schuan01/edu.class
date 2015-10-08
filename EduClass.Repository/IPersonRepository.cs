using EduClass.Entities;

namespace EduClass.Repository
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Person SignIn(string userName, string password);
    }
}
