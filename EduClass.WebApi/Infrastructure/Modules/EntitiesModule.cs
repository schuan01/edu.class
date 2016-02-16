using Autofac;
using EduClass.Entities;
using EduClass.Repository;
using System.Data.Entity;

namespace EduClass.WebApi.Infrastructure.Modules
{
    public class EntitiesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType(typeof(EduClassContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
        }
    }
}