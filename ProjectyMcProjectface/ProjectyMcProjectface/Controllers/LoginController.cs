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
            var inputValidation = InjectionKernel.Instance.Get<IUserManager>();
            string[] errorMessages = inputValidation.ValidateRegisterData(registrationModel.Username, registrationModel.Password, registrationModel.RepeatedPassword, registrationModel.Email);

            if(errorMessages[0] == null && errorMessages[1] == null && errorMessages[2] == null && errorMessages[3] == null)
            {
            }

            return View();
        }
    }
}