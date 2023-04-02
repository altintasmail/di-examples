using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication4.IOC;

namespace WebApplication4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new MyResolver(ConfigureServices()));
        }
        private IServiceProvider ConfigureServices()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddTransient<ILogService, LogService>(); //servisleriniz örneğin ILogService

            var list = typeof(WebApiApplication).Assembly.GetExportedTypes()
           .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
           .Where(t => typeof(IController).IsAssignableFrom(t)
           || typeof(ApiController).IsAssignableFrom(t)
              || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
              || t.Name.EndsWith("ApiController", StringComparison.OrdinalIgnoreCase));
            collection.AddControllersAsServices(list);

            return collection.BuildServiceProvider();
        }

    }
}
