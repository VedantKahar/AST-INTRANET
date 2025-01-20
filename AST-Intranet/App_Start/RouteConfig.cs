using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AST_Intranet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "loginRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "accountsController", action = "loginView", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "signupRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "accountsController", action = "signupView", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "forgotpasswordRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "accountsController", action = "forgotpasswordView", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                 name: "dashboardRoute",
                 url: "{ controller}/{action}/{id}",
                 defaults: new { controller = "Dashboard", action = "DashboardView", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "EmployeesRoute",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Employees", action = "employeesView", id = UrlParameter.Optional }
         );
        }
    }
}
