using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class EventServices : EntityService<Event>, IEventServices
    {
        
        IUnitOfWork _unitOfWork;
        IEventRepository _eventRepository;

        public EventServices(IUnitOfWork unitOfWork, IEventRepository eventRepository)
            : base(unitOfWork, eventRepository)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = eventRepository;
        }

       
    }
}
