using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Logic
{
    public interface IBoardServices : IEntityService<Group>
    {
        IList<Post> GetPosts(int id);
    }
}
