using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;

namespace WebApplication3.Helpers
{
    public class MyResolver : IDependencyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public MyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public object GetService(Type serviceType)
        {
            var scope = GetLifeTimeScop();
            return scope.ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var scope = GetLifeTimeScop();
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