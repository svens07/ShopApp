﻿namespace AuthApi.Models.Requests
{
    public class RegisterRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
