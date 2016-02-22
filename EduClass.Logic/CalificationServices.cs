using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class CalificationServices : EntityService<Calification>, ICalificationServices
    {

        IUnitOfWork _unitOfWork;
        ICalificationRepository _calificationRepository;

        public CalificationServices(IUnitOfWork unitOfWork, ICalificationRepository calificationRepository)
            : base(unitOfWork, calificationRepository)
        {
            _unitOfWork = unitOfWork;
            _calificationRepository = calificationRepository;
        }
    }
}