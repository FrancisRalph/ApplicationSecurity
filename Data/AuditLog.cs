﻿using System;

namespace ApplicationSecurity.Data
{
    public enum LogAction
    {
        SuccessfulLogin,
        WrongPassword,
        Wrong2Fa,
        LogInAttemptButLockedOut,
        Logout
    }
    
    public class AuditLog
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public LogAction Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}