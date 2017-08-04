using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dto
{
    public class UserLogInModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RequestedURL { get; set; }

        public string RegistrationSuccsesMessage { get; set; }

        public string UsernameEmptyFieldError { get; set; }

        public string PasswordEmptyError { get; set; }

        public string BadCredentialsError { get; set; }

        public bool RememberUser { get; set; }
    }

    public class UserRegisterModel
    {
        public string Email { get; set; }
        
        public string UserName { get; set; }

        public string Password { get; set; }

        public string RepeatedPassword { get; set; }

        public string RequestedURL { get; set; }

        public string EmailErrorMessage { get; set; }

        public string UsernameErrorMessage { get; set; }

        public string PasswordErrorMessage { get; set; }
        public string RePasswordErrorMessage { get; set; }
    }

    public class AuthenticatedUserModel : BaseModel
    {
        public string Username { get; set; }
    }
}