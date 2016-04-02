using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;
using System.Collections.Generic;

namespace EduClass.Repository
{
    public class ResponseRepository : BaseRepository<Response>, IResponseRepository
    {
        public ResponseRepository(DbContext context)
            : base(context)
        {

        }

        public IEnumerable<Response> GetResponsesByStudent(Student student) 
        {
            return dbSet.Where(s => s.StudentId == student.Id).ToList();
        }
    }
}
