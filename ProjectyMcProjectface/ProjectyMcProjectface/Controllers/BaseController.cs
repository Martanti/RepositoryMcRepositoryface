using System.Web.Mvc;
using log4net;
using System.Diagnostics;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;

                string userName = claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();

                ViewBag.MainPageLayoutUsername = userName;
            }
                
            base.OnActionExecuting(filterContext);
        }

    }
    
}