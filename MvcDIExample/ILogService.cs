using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebApplication3.Services
{
    public interface ILogService
    {
        void Log(string message);
    }

    public class LogService : ILogService, IDisposable
    {
        public Guid id { get; set; }
        public LogService()
        {
            id = Guid.NewGuid();
            Debug.WriteLine($"{id} Created");
        }
        public void Log(string message)
        {
            Debug.WriteLine($"{message} from {id}");
        }

        public void Dispose()
        {
            Debug.WriteLine($"{id} Disposing");
        }
    }
}