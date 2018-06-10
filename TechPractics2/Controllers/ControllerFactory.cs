using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace TechPractics2.Controllers
{
    public class MyControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return Activator.CreateInstance(typeof(HomeController), new Models.DataManager()) as IController;

            return Activator.CreateInstance(controllerType, new Models.DataManager()) as IController;
        }
    }
}