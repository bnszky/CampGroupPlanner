﻿namespace TripPlanner.Server.Models.Auth
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Token { get; set; }
    }
}
