using ECommerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
    }
}