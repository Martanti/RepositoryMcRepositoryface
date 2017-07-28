using System.Web.Mvc;
using Bussiness;
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