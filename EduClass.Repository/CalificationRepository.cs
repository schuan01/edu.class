using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class CalificationRepository : BaseRepository<Calification>, ICalificationRepository
    {
        public CalificationRepository(DbContext context) : base(context)
        {

        }
    }
}