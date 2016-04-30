using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;
using System.Collections.Generic;

namespace EduClass.Repository
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(DbContext context) : base(context){

        }

        public Group GetByKey(string key)
        {
            return dbSet.FirstOrDefault(x => x.Key == key && x.Enabled == true);
        }

        public IList<Group> GetActiveGroupsByTeacher(int id)
        {
            return GetAll().Include(p => p.Posts).Where(g => g.Enabled && g.Teacher.Id == id).ToList<Group>() ;
        }

        public IList<Group> GetActiveGroupsByStudent(int id)
        {
            return GetAll().Include(p => p.Posts).Where(g => g.Enabled && g.Students.Any(s => s.Id == id)).ToList<Group>();
        }

        public Group GetGroupByIdWithPosts(int id)
        {
            return dbSet.Include(p => p.Posts).FirstOrDefault(g => g.Id == id);
        }

        public void DetachStudent(int id, int studentId)
        {
            context.Database.ExecuteSqlCommand(String.Format("DELETE FROM GroupStudent WHERE Groups_Id = {0} AND Students_Id = {1}", id, studentId));
        }
    }
}
