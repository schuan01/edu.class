using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EduClass.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
