using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(DbContext context) : base(context)
        { }

    }
}
