using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Repository
{
    public class QuestionOptionRepository: BaseRepository<QuestionOption>, IQuestionOptionRepository
    {
		public QuestionOptionRepository(DbContext context)
			: base(context)
        {

        }
    }
}
