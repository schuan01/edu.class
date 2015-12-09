using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class AvatarRepository : BaseRepository<Avatar>, IAvatarRepository
    {
        public AvatarRepository(DbContext context) : base(context)
        {

        }
    }
}
