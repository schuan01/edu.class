using EduClass.Entities;
using System.Collections.Generic;

namespace EduClass.Logic
{
    public interface IGroupServices : IEntityService<Group>
    {
        Group GetByKey(string key);
        IList<Group> GetActiveGroups(Person person);
        Group GetGroupByIdWithPosts(int id);
    }
}
