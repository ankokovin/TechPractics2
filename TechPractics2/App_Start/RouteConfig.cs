using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TechPractics2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "AutoSignIn", id = UrlParameter.Optional },
                new { controller = GetAllControllersAsRegex(), action = GetAllActionsAsRegex() }
            );

            routes.MapRoute(
                name:"NotFound",
                "{*url}",
                new {controller ="Error", action = "NotFound"}
                );
        }

        private static string GetAllControllersAsRegex()
        {
            var controllers = typeof(MvcApplication).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Controller)));

            var controllerNames = controllers
                .Select(c => c.Name.Replace("Controller", ""));

            return string.Format("({0})", string.Join("|", controllerNames));
        }
        private static string GetAllActionsAsRegex()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var actions = asm.GetTypes()
                            .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                            .SelectMany(type => type.GetMethods())
                            .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                            .Select(x => x.Name);

            return string.Format("({0})", string.Join("|", actions));
        }
    }
}

