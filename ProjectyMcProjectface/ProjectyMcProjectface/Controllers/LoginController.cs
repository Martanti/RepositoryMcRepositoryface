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
        [HttpGet]
        public ActionResult Index(string returnUrl, bool IsRegistrySuccessfull = false)
        {
            UserLogInModel userModel = new UserLogInModel();
            userModel.UsernameEmptyFieldError = "";
            userModel.PasswordEmptyError = "";
            userModel.BadCredentialsError = "";
            if(IsRegistrySuccessfull != false)
            {
                userModel.RegistrationSuccsesMessage = "Congratulations! You have successfuly registered!";
            }
            else
            {
                userModel.RegistrationSuccsesMessage = "";
            }
            
            if(returnUrl != null)
            {
                if(HttpContext.Request.Cookies[ReturnUrlCookieName] != null)
                {
                    HttpContext.Response.Cookies[ReturnUrlCookieName].Value = returnUrl;
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(ReturnUrlCookieName);
                    cookie.Value = returnUrl;
                    cookie.Expires = cookie.Expires = DateTime.Now.AddYears(cookieExpirationTimeInYears);
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
            
            return View("Index", userModel);

        }
        
        [HttpPost]
        public ActionResult Index(UserLogInModel userModel)
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

                IUserManager loginAuthenticator = InjectionKernel.Instance.Get<UserManager>();
                if (loginAuthenticator.VerifyLogin(userModel.Email, userModel.Password))
                {
                    var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Email, userModel.Email)
                        },
                        "ApplicationCookie");

                    var owinContext = Request.GetOwinContext();
                    var authManager = owinContext.Authentication;
                    authManager.SignIn(identity);

                    if(HttpContext.Request.Cookies[ReturnUrlCookieName] != null)
                    {
                        if(!String.IsNullOrEmpty(HttpContext.Request.Cookies[ReturnUrlCookieName].Value) &&
                        Url.IsLocalUrl(HttpContext.Request.Cookies[ReturnUrlCookieName].Value))
                        {
                            return Redirect(HttpContext.Request.Cookies[ReturnUrlCookieName].Value);
                        }
                    }
                    else
                    {

                    }
                    return RedirectToAction("Index", "Home");
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