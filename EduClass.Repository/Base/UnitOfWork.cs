﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace EduClass.Repository
{
    /// <summary>
    /// The Entity Framework implementation of IUnitOfWork
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(DbContext context)
        {         

            _dbContext = context;
        }
        
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            //TODO: Eliminar este TODO
            try
            {
                // Save changes with the default options
                return _dbContext.SaveChanges();
            }
            catch (DbUpdateException dbe)
            {
                throw dbe;
            }
            catch (DbEntityValidationException esx)
            {
                foreach (var item in esx.EntityValidationErrors)
                {

                    foreach (var i in item.ValidationErrors)
	                {
                        System.Diagnostics.Debugger.Log(1,"Test", i.ErrorMessage);
	                }
                }

                throw esx;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
