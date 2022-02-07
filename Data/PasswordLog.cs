using System;

namespace ApplicationSecurity.Data
{
    public class PasswordLog
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}