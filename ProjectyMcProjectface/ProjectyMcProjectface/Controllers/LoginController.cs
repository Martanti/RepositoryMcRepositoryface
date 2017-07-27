using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectyMcProjectface.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LogInAction(Dto.UserLogInModel userModel)
        {
            if (userModel.Email == null)
            {
                ViewBag.EmalIsEmpty = "Email field must be filled";
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

            /*Autentikacija ir perėjimas į home page'ą*/
            
            return View("Index");
        }
        [HttpGet]
        public ActionResult RedirectToRegisterPage()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult RegistrationSubmint(Dto.UserRegisterModel registrationModel)
        {
            Bussiness.UserManager inputValidation = new Bussiness.UserManager();
            string[] errorMessages = inputValidation.ValidateRegisterData(registrationModel.Username, registrationModel.Password, registrationModel.RepeatedPassword, registrationModel.Email);

            if(errorMessages[0] == null && errorMessages[1] == null && errorMessages[2] == null && errorMessages[3] == null)
            {

                //įkelimas i serva

                return View("Index");
            }

            ViewBag.BadEmail = errorMessages[3];

            ViewBag.BadUsername = errorMessages[0];

            ViewBag.BadPassword = errorMessages[1];

            ViewBag.BadRePassword = errorMessages[2];

            return View("Register");
        }
    }
}