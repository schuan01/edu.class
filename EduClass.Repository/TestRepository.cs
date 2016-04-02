using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;
using System.Collections.Generic;

namespace EduClass.Repository
{
    public class TestRepository : BaseRepository<Test>, ITestRepository
    {
        public TestRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<Test> GetAll(int id) 
        {
            return dbSet.Where(x => x.GroupId == id);
        }

        public IEnumerable<Test> GetEnabledTestForStudents(int groupId)
        {
            return dbSet.Where(x => x.GroupId == groupId 
                                    && x.Enabled 
                                    && x.Questions.Count() > 0
                                    && (x.StartDate <= DateTime.Now 
                                    && x.EndDate >= DateTime.Now));
        }
    }
}
