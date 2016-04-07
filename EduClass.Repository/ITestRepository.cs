using EduClass.Entities;
using System.Collections.Generic;

namespace EduClass.Repository
{
    public interface ITestRepository : IBaseRepository<Test>
    {
        IEnumerable<Test> GetAll(int id);
        IEnumerable<Test> GetEnabledTestForStudents(int groupId);
        IEnumerable<Test> GetTestStudents(int idStudent);
    }
}
