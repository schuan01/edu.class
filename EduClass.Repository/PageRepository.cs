using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class PageRepository : BaseRepository<Page>, IPageRepository
    {
        public PageRepository(DbContext context) : base(context)
        {

        }   
    }
}
