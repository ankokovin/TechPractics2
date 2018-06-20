using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPractics2.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Intern()
        {
            //Response.StatusCode = 500;
            
            return View(Response.StatusCode);
        }

        public ActionResult NotFound()
        {
            
            return View(Response.StatusCode);
        }
    }
}