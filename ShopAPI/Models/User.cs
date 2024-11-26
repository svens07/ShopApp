﻿namespace ShopAPI.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string role { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? lastLogin { get; set; }
        public bool isEnabled { get; set; }
    }
}
