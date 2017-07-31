using System.Web.Mvc;
using Bussiness;
using System.Security.Claims;
using System.Collections.Generic;
using System.Web;
using System;

namespace ProjectyMcProjectface.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
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
            Dto.UserLogInModel model = new Dto.UserLogInModel();
            model.RequestedURL = returnUrl;
            return View(model);
        }
        
        [HttpPost]
        public ActionResult LogInAction(Dto.UserLogInModel userModel)
        {
            
            if (userModel.Email == null || userModel.Password == null)
            {

                if (userModel.Email == null)
                {
                    ViewBag.EmalIsEmpty = Resources.LoginPageResources.error_message_Email_empty;
                }

                else
                {
                    ViewBag.UsernameIsEmpty = null;
                }

                if (userModel.Password == null)
                {
                    ViewBag.PasswordIsEmpty = Resources.LoginPageResources.error_message_Password_empty;
                }

                else
                {
                    ViewBag.PasswordIsEmpty = null;
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
                    ViewBag.IncorrectCredentials = Resources.LoginPageResources.error_message_Inncorrect_Credentials;
                }
            }

            return View("Index", userModel);
        }

    }
}