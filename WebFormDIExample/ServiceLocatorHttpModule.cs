using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.HttpModules
{
    public class ServiceLocatorHttpModule : IHttpModule
    {
        public IServiceScope Scope { get; internal set; }
        public void Dispose()
        {
            Scope.Dispose();
        }

        public void Init(HttpApplication context)
        {
            Scope = Global.ServiceProvider.CreateScope();
            //context.BeginRequest += new System.EventHandler(Begin);           
        }

        //private void Begin(Object Sender, EventArgs e)
        //{
        //}
    }
}