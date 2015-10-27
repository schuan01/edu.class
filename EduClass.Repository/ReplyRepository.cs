using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class ReplyRepository : BaseRepository<Reply>, IReplyRepository
    {
        public ReplyRepository(DbContext context) : base(context)
        {

        }   
    }
}
