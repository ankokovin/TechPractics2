using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace TechPractics2
{
    public class HandleCustomError : HandleErrorAttribute
    {

        public override void OnException(ExceptionContext filterContext)
        {
            //If the exeption is already handled we do nothing
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {
                //Log the exception with Elmah
                Log(filterContext);

                Type exceptionType = filterContext.Exception.GetType();

                    base.OnException(filterContext);
                

            }

            //Make sure that we mark the exception as handled
            filterContext.ExceptionHandled = true;
        }

        private void Log(ExceptionContext context)
        {
            // Retrieve the current HttpContext instance for this request.
            HttpContext httpContext = context.HttpContext.ApplicationInstance.Context;

            if (httpContext == null)
            {
                return;
            }

            // Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
            Exception exceptionToRaise = new HttpUnhandledException(message: null, innerException: context.Exception);

            // Send the exception to ELMAH (for logging, mailing, filtering, etc.).
            ErrorSignal signal = ErrorSignal.FromContext(httpContext);
            signal.Raise(exceptionToRaise, httpContext);
        }

    }
}