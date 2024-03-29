﻿namespace TodoAPI.Models
{
    public class UserInfo
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? IpAddress { get; set; }
    }
}
