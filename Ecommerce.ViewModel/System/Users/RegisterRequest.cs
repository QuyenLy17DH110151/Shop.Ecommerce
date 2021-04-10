﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.System.Users
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
