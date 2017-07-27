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
            if (userModel.UserName == null)
            {
                ViewBag.UsernameIsEmpty = "Username field must be filled";
            }

            else
            {
                ViewBag.UsernameIsEmpty = null;
            }

            if (userModel.Password == null)
            {
                ViewBag.PasswordIsEmpty = "Password field must be filled";
            }

            else
            {
                ViewBag.PasswordIsEmpty = null;
            }


            return View("Index");
        }
    }
}