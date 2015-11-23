using EduClass.Entities;

namespace EduClass.Logic
{
    public interface IGroupServices : IEntityService<Group>
    {
        Group GetByKey(string key);
    }
}
