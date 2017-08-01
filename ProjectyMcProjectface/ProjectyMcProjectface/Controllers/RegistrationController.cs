using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bussiness;
using Dto;

namespace ProjectyMcProjectface.Controllers
{
    [AllowAnonymous]
    public class RegistrationController : BaseController
    {
        
        [HttpGet]
        public ActionResult Register()
        {
            UserRegisterModel userModel = new UserRegisterModel();
            userModel.EmailErrorMessage = "";
            userModel.UsernameErrorMessage = "";
            userModel.PasswordErrorMessage = "";
            userModel.RePasswordErrorMessage = "";
            return View("Register", userModel);
        }

        [HttpPost]
        public ActionResult RegistrationSubmint(UserRegisterModel registrationModel)
        {
            registrationModel.EmailErrorMessage = "";
            registrationModel.UsernameErrorMessage = "";
            registrationModel.PasswordErrorMessage = "";
            registrationModel.RePasswordErrorMessage = "";

            var inputValidation = InjectionKernel.Instance.Get<IUserManager>();
            string[] errorMessages = inputValidation.ValidateRegisterData(registrationModel.Username, registrationModel.Password, registrationModel.RepeatedPassword, registrationModel.Email);

            if (errorMessages[0] == "" && errorMessages[1] == "" && errorMessages[2] == "" && errorMessages[3] == "")
            {
                IUserManager userRegistration = InjectionKernel.Instance.Get<UserManager>();

                userRegistration.RegisterUser(registrationModel.Username, registrationModel.Password, registrationModel.Email);

                return RedirectToAction("Index", "Login", new { RegistrationSuccessful = true });
            }

            else
            {
                registrationModel.EmailErrorMessage = errorMessages[3];
                registrationModel.UsernameErrorMessage = errorMessages[0];
                registrationModel.PasswordErrorMessage = errorMessages[1];
                registrationModel.RePasswordErrorMessage = errorMessages[2];
            }

            return View("Register", registrationModel);
        }

    }
}