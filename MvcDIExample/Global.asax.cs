using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication3.Extensions;
using WebApplication3.Helpers;
using WebApplication3.Services;

namespace WebApplication3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(new MyResolver(ConfigureServices()));
        }

        private IServiceProvider ConfigureServices()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddTransient<ILogService, LogService>(); //servisleriniz örneğin ILogService

            collection.AddControllersAsServices(typeof(MvcApplication).Assembly.GetExportedTypes()
           .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
           .Where(t => typeof(IController).IsAssignableFrom(t)
              || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            return collection.BuildServiceProvider();
        }
    }
}
