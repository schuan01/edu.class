using EduClass.Entities;

namespace EduClass.Logic
{
    public interface IPersonServices : IEntityService<Person>
    {
        Person SignIn(string userName, string password);
    }
}
