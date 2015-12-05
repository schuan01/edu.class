using EduClass.Entities;
using System.Collections.Generic;

namespace EduClass.Repository
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Group GetByKey(string key);
        Group GetGroupByIdWithPosts(int id);
        IList<Group> GetActiveGroupsByTeacher(int id);
        IList<Group> GetActiveGroupsByStudent(int id);
    }
}
