﻿namespace ConvicartAdminApp.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; } // Admin, CustomerCareExec, DeliveryPartner
    }

}
