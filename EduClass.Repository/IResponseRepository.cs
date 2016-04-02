using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduClass.Repository
{
    public interface IResponseRepository : IBaseRepository<Response>
    {
        IEnumerable<Response> GetResponsesByStudent(int idStudent);
        IList<Student> GetStudentsTests(int idTest);
    }
}
