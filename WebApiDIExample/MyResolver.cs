using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication4.IOC
{
    public class MyResolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private IServiceProvider serviceProvider { get; set; }
        private IServiceScope serviceScope { get; set; }

        public MyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public MyResolver(IServiceScope serviceScope)
        {
            this.serviceScope = serviceScope;
        }

        public IDependencyScope BeginScope()
        {
            return new MyResolver(serviceProvider.CreateScope());
        }

        public void Dispose()
        {
            serviceScope?.Dispose();
        }

        public object GetService(Type serviceType)
        {
            var scope = serviceScope ?? GetLifeTimeScop();
            return scope.ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var scope = serviceScope ?? GetLifeTimeScop();
            return scope.ServiceProvider.GetServices(serviceType);
        }

        private IServiceScope GetLifeTimeScop()
        {
            var currentHttpContext = HttpContext.Current;
            if (currentHttpContext != null)
            {
                var lifetimeScope = (IServiceScope)currentHttpContext.Items[typeof(IServiceScope)];
                if (lifetimeScope == null)
                {
                    void CleanScope(object sender, EventArgs args)
                    {
                        if (sender is HttpApplication application)
                        {
                            application.RequestCompleted -= CleanScope;
                            lifetimeScope.Dispose();
                            Debug.WriteLine("Clean");
                        }
                    }

                    lifetimeScope = serviceProvider.CreateScope();
                    currentHttpContext.Items.Add(typeof(IServiceScope), lifetimeScope);
                    currentHttpContext.ApplicationInstance.RequestCompleted += CleanScope;
                    Debug.WriteLine("Create");
                }

                return lifetimeScope;
            }

            return null;
        }
    }
}