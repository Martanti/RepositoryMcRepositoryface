using System.Web.Mvc;
using log4net;
using System.Diagnostics;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Dto;
using Bussiness;
using System.Web;

namespace ProjectyMcProjectface.Controllers
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                throw new HttpException(404, "HTTP/1.1 404 Not Found");
        }
    }
    public class BaseController : Controller
    {
        public readonly string SelectedDatabaseCookieName = "SelectedDatabase";
        public readonly string EmailCookieName = "UserEmail";
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
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            

            base.OnActionExecuted(filterContext);

            if (User.Identity.IsAuthenticated && filterContext.Controller.ViewData.Model != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;

                IDatabaseManager DBManager = InjectionKernel.Instance.Get<IDatabaseManager>();

                var userName = claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
                var email = claims.Single(c => c.Type == ClaimTypes.Email).Value.ToString();
                var model = filterContext.Controller.ViewData.Model as BaseModel;

                model.UserName = userName;
                model.UserDatabases = DBManager.GetDatabasesByEmail(email);
            }
        }

    }
    
}