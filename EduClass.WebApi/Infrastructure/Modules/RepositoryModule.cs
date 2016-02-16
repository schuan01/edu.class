using Autofac;

namespace EduClass.WebApi.Infrastructure.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
             builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("EduClass.Repository"))
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();
        }
    }
}