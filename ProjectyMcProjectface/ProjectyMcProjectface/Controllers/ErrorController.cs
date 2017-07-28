using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectyMcProjectface.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Handle403()
        {
            Response.StatusCode = 403;
            return View("Handle403");
        }
        public ActionResult Handle404()
        {
            Response.StatusCode = 404;
            return View("Handle404");
        }

        public ActionResult Handle500()
        {
            Response.StatusCode = 500;
            return View("Handle500");
        }
        public ActionResult Handle505()
        {
            Response.StatusCode = 505;
            return View("Handle505");
        }

    }
}