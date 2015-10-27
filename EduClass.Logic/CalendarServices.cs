using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class CalendarServices : EntityService<Calendar>, ICalendarServices
    {
        
        IUnitOfWork _unitOfWork;
        ICalendarRepository _calendarRepository;

        public CalendarServices(IUnitOfWork unitOfWork, ICalendarRepository calendarRepository)
            : base(unitOfWork, calendarRepository)
        {
            _unitOfWork = unitOfWork;
            _calendarRepository = calendarRepository;
        }
    }
}
