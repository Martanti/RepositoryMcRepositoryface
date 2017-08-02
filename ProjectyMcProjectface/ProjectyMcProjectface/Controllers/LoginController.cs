using System.Web.Mvc;
using Bussiness;
using System.Security.Claims;
using System.Collections.Generic;
using System.Web;
using System;
using Dto;

namespace ProjectyMcProjectface.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        public readonly double RememberMeExpirationTimeInDays = 7;
        [HttpGet]
        public ActionResult Login(string returnUrl = null, bool RegistrationSuccessful = false)
        {
            UserLogInModel userModel = new UserLogInModel();
            userModel.UsernameEmptyFieldError = "";
            userModel.PasswordEmptyError = "";
            userModel.BadCredentialsError = "";
            if(RegistrationSuccessful != false)
            {
                userModel.RegistrationSuccsesMessage = "Congratulations! You have successfuly registered!";
            }
            else
            {
                userModel.RegistrationSuccsesMessage = "";
            }
            
            /*enter valuie to model*/
            
            return View("Index", userModel);

        }
        
        [HttpPost]
        public ActionResult Login(UserLogInModel userModel)
        {

            userModel.UsernameEmptyFieldError = "";
            userModel.PasswordEmptyError = "";
            userModel.BadCredentialsError = "";
            userModel.RegistrationSuccsesMessage = "";
            
            if (userModel.Email == null || userModel.Password == null)
            {

                if (userModel.Email == null)
                {
                    userModel.UsernameEmptyFieldError = Resources.LoginPageResources.error_message_Email_empty;
                }

                if (userModel.Password == null)
                {
                    userModel.PasswordEmptyError = Resources.LoginPageResources.error_message_Password_empty;
                }
            }
            else
            {

                IUserManager loginAuthenticator = InjectionKernel.Instance.Get<IUserManager>();
                if (loginAuthenticator.VerifyLogin(userModel.Email, userModel.Password))
                {
                    var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Email, userModel.Email)
                        },
                        "ApplicationCookie");
                    

                    var owinContext = Request.GetOwinContext();
                    var authManager = owinContext.Authentication;

                    if (userModel.RememberUser)
                    {
                        authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) }, identity);
                    }
                    else
                    {
                        authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = true}, identity);
                    }

                    if (!String.IsNullOrEmpty(/*modelio url reiksme*/) &&
                       Url.IsLocalUrl(/*modelio url reiksme*/))
                    {
                        IUserManager usernameGetter = InjectionKernel.Instance.Get<IUserManager>();
                        return RedirectToAction("Index", "Home", new { Username = usernameGetter.GetUsernameByEmail(userModel.Email) });
                    }

                }

                else
                {
                    userModel.BadCredentialsError = Resources.LoginPageResources.error_message_Inncorrect_Credentials;
                }
            }

            return View("Index", userModel);
        }

    }
}