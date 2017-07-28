using System.Web.Mvc;
using log4net;
using System.Diagnostics;

namespace ProjectyMcProjectface.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(ErrorController));

            var st = new StackTrace(filterContext.Exception, true);
            log.Error(filterContext.Exception.Message);
            /*base.OnException(filterContext);*/
        }
    }
}