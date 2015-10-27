using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class CalendarRepository : BaseRepository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(DbContext context) : base(context)
        {

        }   
    }
}
