using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.IOC;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogService logService;

        public HomeController(ILogService logService)
        {
            this.logService = logService;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            logService.Log(nameof(Index));
            return View();
        }
    }
}
