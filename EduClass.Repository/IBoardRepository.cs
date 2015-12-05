using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Repository
{
    public interface IBoardRepository : IBaseRepository<Group>
    {
        IList<Post> GetPosts(int id);
    }
}
