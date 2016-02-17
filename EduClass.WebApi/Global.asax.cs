using Autofac;
using Autofac.Integration.WebApi;
using EduClass.Entities;
using EduClass.WebApi.Infrastructure.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace EduClass.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Autofac Configuration
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());

            builder.RegisterModule(new EntitiesModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());

            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public class PersonBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var contentType = controllerContext.HttpContext.Request.ContentType;
                if (!contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                    return (null);

                string bodyText;

                using (var stream = controllerContext.HttpContext.Request.InputStream)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                        bodyText = reader.ReadToEnd();
                }

                if (string.IsNullOrEmpty(bodyText)) return (null);

                var xx = new JavaScriptSerializer().Deserialize<Person>(bodyText);

                return xx;
            }
        }
    }
}
