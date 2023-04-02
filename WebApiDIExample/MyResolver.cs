using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication4.IOC
{
    public class MyResolver : DefaultControllerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public MyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        protected override IController GetControllerInstance(
                RequestContext requestContext, Type controllerType)
        {
            IServiceScope scope = GetLifeTimeScop();

            return (IController)scope.ServiceProvider.GetRequiredService(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);

            var scope = GetLifeTimeScop();

            scope?.Dispose();
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