using EduClass.Entities;

namespace EduClass.Repository
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Person SignIn(string userName, string password);
        Person GetByUserNameAndMail(string userName, string email);
        Person GetByUserName(string userName);
        Person GetByEmail(string email);
    }
}
