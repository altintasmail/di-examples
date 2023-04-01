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
            var scope = (IServiceScope)HttpContext.Current.Items[typeof(IServiceScope)];
            return scope.ServiceProvider.GetService<T>();
        }

        public static T GetRequiredService<T>()
        {
            var scope = (IServiceScope)HttpContext.Current.Items[typeof(IServiceScope)];
            return scope.ServiceProvider.GetRequiredService<T>();
        }
        public object GetService(Type serviceType)
        {
            //Debug.WriteLine(serviceType.Name);
            var scope = (IServiceScope)HttpContext.Current?.Items[typeof(IServiceScope)];
            try
            {
                //önce service provider veya public create instance denenir
                return ActivatorUtilities.GetServiceOrCreateInstance(scope == null ? Global.ServiceProvider : scope.ServiceProvider, serviceType);
            }
            catch (InvalidOperationException)
            {
                //yoksa herhangi bir constructor ile oluşturma denenir.
                return Activator.CreateInstance(serviceType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
            }
        }
    }
}