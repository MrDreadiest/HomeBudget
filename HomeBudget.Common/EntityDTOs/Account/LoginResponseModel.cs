﻿namespace HomeBudget.Common.EntityDTOs.Account
{
    public class LoginResponseModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Email { get; set; }
    }
}