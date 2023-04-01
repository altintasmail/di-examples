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
            var locator = (ServiceLocatorHttpModule)HttpContext.Current.ApplicationInstance.Modules[nameof(ServiceLocatorHttpModule)];
            return locator.Scope.ServiceProvider.GetService<T>();
        }

        public static T GetRequiredService<T>()
        {
            var locator = (ServiceLocatorHttpModule)HttpContext.Current.ApplicationInstance.Modules[nameof(ServiceLocatorHttpModule)];
            return locator.Scope.ServiceProvider.GetRequiredService<T>();
        }
        public object GetService(Type serviceType)
        {
            //Debug.WriteLine(serviceType.Name);
            var locator = (ServiceLocatorHttpModule)HttpContext.Current?.ApplicationInstance?.Modules[nameof(ServiceLocatorHttpModule)];
            try
            {
                //önce service provider veya public create instance denenir
                return ActivatorUtilities.GetServiceOrCreateInstance(locator == null ? Global.ServiceProvider : locator.Scope.ServiceProvider, serviceType);
            }
            catch (InvalidOperationException)
            {
                //yoksa herhangi bir constructor ile oluşturma denenir.
                return Activator.CreateInstance(serviceType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
            }
        }
    }
}