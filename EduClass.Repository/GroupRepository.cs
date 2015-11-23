using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(DbContext context) : base(context)
        {

        }
        public Group GetByKey(string key)
        {
            return dbSet.Where(x => x.Key == key).FirstOrDefault();
        }



    }
}
