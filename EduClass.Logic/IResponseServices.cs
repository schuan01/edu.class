using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduClass.Logic
{
    public interface IResponseServices : IEntityService<Response>
    {
        IEnumerable<Response> GetResponsesByStudent(int idStudent);
        IList<Student> GetStudentsTests(int idTest);
    }
}
