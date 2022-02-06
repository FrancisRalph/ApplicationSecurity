﻿using System;

namespace ApplicationSecurity.Data
{
    public enum LogAction
    { // starts at index 0
        SuccessfulLogin,
        WrongPassword,
        Wrong2Fa,
        RedirectedToLockedOut,
        RedirectedTo2Fa,
        Logout
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