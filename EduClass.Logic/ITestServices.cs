using EduClass.Entities;
using System.Collections.Generic;

namespace EduClass.Logic
{
    public interface ITestServices : IEntityService<Test>
    {
        IEnumerable<Test> GetAll(int id);
        IEnumerable<Test> GetEnabledTestForStudents(int groupId);
        IEnumerable<Test> GetTestStudents(int idStudent);
    }
}
