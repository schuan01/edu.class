using Autofac;

namespace EduClass.Web.Infrastructure.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("EduClass.Logic"))
                  .Where(t => t.Name.EndsWith("Services"))
                  .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();
        }
    }
}