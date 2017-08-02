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
        public ActionResult Index(AuthenticatedUserModel userModel = null, string Username = "")
        {
            userModel.Username = Username;
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
        public ActionResult AddDatabase()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyConnectionString()
        {
            return RedirectToAction("AddDatabase");
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