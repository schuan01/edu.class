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

        public IEnumerable<Response> GetResponsesByStudent(int idStudent, int idTest) 
        {
            return dbSet.Where(s => s.StudentId == idStudent && s.Question.TestId == idTest).ToList();
        }

        public IEnumerable<Response> GetResponsesByStudent(int idStudent)
        {
            return dbSet.Where(s => s.StudentId == idStudent).ToList();
        }

        public IList<Student> GetStudentsTests(int idTest)
        {
            var student = new List<Student>();

            foreach(var item in dbSet.Where(x => x.Question.TestId == idTest))
            {
                if (!student.Any(s => s.Id == item.StudentId))
                {
                    student.Add(item.Student);
                }
            };

            return student;
        }

        public int GetCorrectResponses(int idStudent, int idTest)
        {
            var r = GetResponsesByStudent(idStudent, idTest);

            var a = r.Where(x => x.IsCorrect == true).GroupBy(x => x.QuestionId).Select(g => g.First());

            return a.Count();
        }
    }
}
