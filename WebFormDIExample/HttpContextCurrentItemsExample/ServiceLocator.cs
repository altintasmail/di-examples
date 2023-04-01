using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebApplication2.HttpModules;

namespace WebApplication2.Helpers
{
    public class ServiceLocator : IServiceProvider
    {
        public static T GetService<T>()
        {
            var scope = GetLifeTimeScop();
            return scope.ServiceProvider.GetService<T>();
        }

        public static T GetRequiredService<T>()
        {
            var scope = GetLifeTimeScop();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
        public object GetService(Type serviceType)
        {
            try
            {
                IServiceScope lifetimeScope = GetLifeTimeScop();         
                //önce service provider veya public create instance denenir
                return ActivatorUtilities.GetServiceOrCreateInstance(lifetimeScope == null ? Global.ServiceProvider : lifetimeScope.ServiceProvider, serviceType);
            }
            catch (InvalidOperationException)
            {
                //yoksa herhangi bir constructor ile oluşturma denenir.
                return Activator.CreateInstance(serviceType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
            }
        }

        private static IServiceScope GetLifeTimeScop()
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

                    lifetimeScope = Global.ServiceProvider.CreateScope();
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