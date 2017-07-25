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
        [DisplayName("User Name")]
        [Required(ErrorMessage = "This field is required to be filled")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "This field is required to be filled")]
        public string Password { get; set; }

        public string LoginErrorMessage { get; set; }
    }
}