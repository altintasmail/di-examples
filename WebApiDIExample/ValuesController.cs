using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebApplication4.IOC;

namespace WebApplication4.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ILogService logService;

        public ValuesController(ILogService logService)
        {
            this.logService = logService;
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            logService.Log(nameof(Get));
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
