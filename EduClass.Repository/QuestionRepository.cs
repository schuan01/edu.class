using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Repository
{
    public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(DbContext context) : base(context)
        {

        }

        public IQueryable<Question> GetAll(int id)
        {
            return dbSet.Where(q => q.TestId == id && q.Enabled).OrderByDescending(q => q.CreatedAt);
        }
    }
}
