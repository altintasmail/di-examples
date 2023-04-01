using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication2.Helpers;

namespace WebApplication2
{
    public partial class _Default : Page
    {
        private readonly ILogService logService;

        public _Default(ILogService logService)//Dependency injection yoluyla alması sağlandı
        {
            this.logService = logService;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var s1 = ServiceLocator.GetService<ILogService>();
            var s2 = ServiceLocator.GetService<ILogService>();
            s1.Log("test1");
            s2.Log("test2");
            logService.Log("constructor injection test");
        }

        [WebMethod(enableSession: true)]
        public static List<User> GetUsers()
        {
            var factory = ServiceLocator.GetRequiredService<IHttpClientFactory>();
            using (var client = factory.CreateClient("ApiHttpclient"))
            {
                var result = client.GetAsync("/ListUsers").GetAwaiter().GetResult();
                var data = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<List<User>>(data);
            }
        }
    }
}