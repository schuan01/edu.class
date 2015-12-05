using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Repository
{
    public class BoardRepository : BaseRepository<Group>, IBoardRepository
    {
        public BoardRepository(DbContext context)
            : base(context)
        { }

        public IList<Post> GetPosts(int id)
        {
            var group = dbSet.Include(b => b.Posts).FirstOrDefault(g => g.Id == id);

            return group.Posts.OrderByDescending(x => x.CreatedAt).Take(20).ToList();
        }
    }
}
