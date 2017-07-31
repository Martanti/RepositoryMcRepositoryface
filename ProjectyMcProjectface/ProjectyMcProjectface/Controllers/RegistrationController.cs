using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bussiness;

namespace ProjectyMcProjectface.Controllers
{
    public class RegistrationController : BaseController
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult RegistrationSubmint(Dto.UserRegisterModel registrationModel)
        {
            var inputValidation = InjectionKernel.Instance.Get<IUserManager>();
            string[] errorMessages = inputValidation.ValidateRegisterData(registrationModel.Username, registrationModel.Password, registrationModel.RepeatedPassword, registrationModel.Email);

            if (errorMessages[0] == null && errorMessages[1] == null && errorMessages[2] == null && errorMessages[3] == null)
            {
                IUserManager userRegistration = InjectionKernel.Instance.Get<UserManager>();

                userRegistration.RegisterUser(registrationModel.Username, registrationModel.Password, registrationModel.Email);

                return RedirectToAction("Index", "Login");
            }

            else
            {
                ViewBag.BadEmail = errorMessages[3];
                ViewBag.BadUsername = errorMessages[0];
                @ViewBag.BadPassword = errorMessages[1];
                @ViewBag.BadRePassword = errorMessages[2];
            }

            return View("Register");
        }

    }
}