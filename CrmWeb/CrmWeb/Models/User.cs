﻿namespace CrmWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PLZ { get; set; }
        public string? Phone { get; set; }
        public string? CreationDate { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
