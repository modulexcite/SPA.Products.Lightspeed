using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Products.Entities.Models;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Logging;
using Mindscape.LightSpeed.Web.Mvc;
using Ninject.Web.Common;
using Ninject;
using Products.App.Infrastructure;
using System.Reflection;
using System.Web.Http;

namespace Products.App
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static readonly LightSpeedContext<DbUnitOfWork> LightSpeedContext
  = new LightSpeedContext<DbUnitOfWork>("Products") { IdentityMethod = IdentityMethod.IdentityColumn };

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                "Default apiRoute",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configuration.Formatters.RemoveAt(1);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            LightSpeedEntityModelBinder.Register(typeof(DbUnitOfWork).Assembly);
        }
    }
}