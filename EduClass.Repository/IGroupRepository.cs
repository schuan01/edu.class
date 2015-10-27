using EduClass.Entities;

namespace EduClass.Repository
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        bool Join(Person _person);
    }
}
