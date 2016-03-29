using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Logic
{
    public interface IQuestionServices : IEntityService<Question>
    {
        IQueryable<Question> GetAll(int id);
    }
}
