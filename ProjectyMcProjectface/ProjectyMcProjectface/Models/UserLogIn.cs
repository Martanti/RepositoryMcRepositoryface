using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectyMcProjectface.Models
{
    public class UserLogIn
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}