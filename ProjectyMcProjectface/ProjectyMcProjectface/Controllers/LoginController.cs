using System.Web.Mvc;
using Bussiness;
using Dto;
namespace ProjectyMcProjectface.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string dipshitoMessage = "", bool IsRegistrySuccessfull = false)
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