using System.Web.Mvc;
using log4net;
using System.Diagnostics;
using System;

namespace ProjectyMcProjectface.Controllers
{
    public class BaseController : Controller
    {
        public readonly string ReturnUrlCookieName = "returnUrl";
        public readonly int cookieExpirationTimeInYears = 15;
        protected override void OnException(ExceptionContext filterContext)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(ErrorController));

            var st = new StackTrace(filterContext.Exception, true);
            log.Error(filterContext.Exception.ToString()+ Environment.NewLine +
                "---------------------------------------------------------------------------------------------------" +
                "---------------------------------------------------------------------------------------------------");
        }
    }
}