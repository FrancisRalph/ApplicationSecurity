#nullable enable
using System;

namespace ApplicationSecurity.Data
{
    public enum LogAction
    { // starts at index 0
        SuccessfulLogin, // 0
        WrongPassword, // 1
        Wrong2Fa, // 2
        RedirectedToLockedOut, // 3
        RedirectedTo2Fa, // 4
        Logout // 5
    }
    
    public class AuditLog
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public LogAction Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
    }
}