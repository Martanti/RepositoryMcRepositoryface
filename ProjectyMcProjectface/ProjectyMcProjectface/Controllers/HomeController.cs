using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dto;

namespace ProjectyMcProjectface.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(AuthenticatedUserModel userModel)
        {
            return View("Index", userModel);
        }

        public ActionResult DatabaseEdit()
        {
            return View();
        }

        public ActionResult DatabaseView()
        {
            return View();
        }
        public ActionResult SignOut()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Login");
        }
    }
}