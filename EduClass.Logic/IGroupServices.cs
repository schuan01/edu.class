using EduClass.Entities;

namespace EduClass.Logic
{
    public interface IGroupServices : IEntityService<Group>
    {
        bool Join(Person _person);
    }
}
