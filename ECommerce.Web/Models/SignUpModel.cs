﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{
    public class SignUpModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RePassword { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
    }
}