using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class TestRepository : BaseRepository<Test>, ITestRepository
    {
        public TestRepository(DbContext context) : base(context)
        {

        }
    }
}
