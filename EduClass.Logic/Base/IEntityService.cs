﻿using System.Collections.Generic;

namespace EduClass.Logic
{
    public interface IEntityService<T> : IService where T : class
    {
        void Create(T entity);
        void Delete(T entity);
        void Delete(int id);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Update(T entity);
    }
}
