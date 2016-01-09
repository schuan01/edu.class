using System;
using System.Collections.Generic;
using EduClass.Repository;

namespace EduClass.Logic
{
    public abstract class EntityService<T> : IEntityService<T> where T : class
    {
        IUnitOfWork _unitOfWork;
        IBaseRepository<T> _repository;

        public EntityService(IUnitOfWork unitOfWork, IBaseRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual void Create(T entity)
        {
            if (entity == null) { throw new ArgumentNullException("entity"); }

            _repository.Add(entity);
            _unitOfWork.Commit();
        }

        public virtual void Update(T entity)
        {
            if (entity == null) { throw new ArgumentNullException("entity"); }

            _repository.Update(entity);
            _unitOfWork.Commit();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) { throw new ArgumentNullException("entity"); }

            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual void Delete(int id)
        {
            if (id == null) { throw new ArgumentNullException("entity"); }

            _repository.Delete(id);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }


        public T GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
