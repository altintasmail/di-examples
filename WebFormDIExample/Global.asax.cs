using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using Services;
using WebApplication2.HttpModules;
using System.Configuration;
using WebApplication2.HttpClientHandlers;
using WebApplication2.Helpers;

namespace WebApplication2
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigureServices(); //dependency düzenlemesinin etkinleştirilmesi
            //Constructor injection için
            HttpRuntime.WebObjectActivator = new ServiceLocator();
        }

        public static IServiceProvider ServiceProvider { get; set; }

        private void ConfigureServices()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddTransient<ILogService, LogService>(); //servisleriniz örneğin ILogService

            //http client için "Microsoft.Extensions.Http" paketi eklenmelidir
            collection.AddTransient<ApiHttpClientMessageHandler>();
            collection.AddHttpClient("ApiHttpclient", options =>
            {
                options.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]);
            }).AddHttpMessageHandler<ApiHttpClientMessageHandler>();

            ServiceProvider = collection.BuildServiceProvider();
        }
    }
}