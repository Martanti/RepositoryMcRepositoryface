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
    }

    public class UserRegisterModel
    {
        public string Email { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public string RepeatedPassword { get; set; }
    }
}