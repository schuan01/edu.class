using EduClass.Entities;

namespace EduClass.Repository
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Group GetByKey(string key);
    }
}
