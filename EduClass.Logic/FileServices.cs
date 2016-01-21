using System;
using System.Collections.Generic;
using EduClass.Entities;
using EduClass.Repository;

namespace EduClass.Logic
{
    public class FileServices : EntityService<File>, IFileServices
    {

        IUnitOfWork _unitOfWork;
        IFileRepository _fileRepository;

        public FileServices(IUnitOfWork unitOfWork, IFileRepository fileRepository)
            : base(unitOfWork, fileRepository)
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
        }
    }
}
