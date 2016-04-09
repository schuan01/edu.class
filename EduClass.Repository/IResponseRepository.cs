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
        IEnumerable<Response> GetResponsesByStudent(int idStudent, int idTest);
        IList<Student> GetStudentsTests(int idTest);
        int GetCorrectResponses(int idStudent, int idTest);
    }
}
