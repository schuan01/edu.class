using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class PageServices : EntityService<Page>, IPageServices
    {
        
        IUnitOfWork _unitOfWork;
        IPageRepository _pageRepository;

        public PageServices(IUnitOfWork unitOfWork, IPageRepository pageRepository)
            : base(unitOfWork, pageRepository)
        {
            _unitOfWork = unitOfWork;
            _pageRepository = pageRepository;
        }
    }
}
