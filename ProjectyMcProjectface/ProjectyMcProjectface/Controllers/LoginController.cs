using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectyMcProjectface.Models;

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
        public ActionResult Authorize(ProjectyMcProjectface.Models.RegisteredUser1 userModel)
        {
            using (InternalDBEntities1 db = new InternalDBEntities1())
            {
                var userDetails = db.RegisteredUsers.Where(x => x.UserName == userModel.UserName && x.PassWord == userModel.PassWord).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Your password or user name is incorrect";
                    return View("Index", userModel);
                }

            }

                return View();
        }
    }
}