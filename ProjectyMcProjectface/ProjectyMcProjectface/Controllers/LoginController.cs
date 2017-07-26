using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectyMcProjectface.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LogInAction(ProjectyMcProjectface.Models.UserLogIn userModel)
        {
            userModel.LoginErrorMessage = "Login failed. Password and/or username is incorrect";

            return View("Index", userModel);
        }
    }
}