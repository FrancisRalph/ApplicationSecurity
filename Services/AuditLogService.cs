using System;
using System.Threading.Tasks;
using ApplicationSecurity.Data;
using Microsoft.AspNetCore.Http;

namespace ApplicationSecurity.Services
{
    public class AuditLogService
    {
        private readonly ApplicationDbContext _context;

        public AuditLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAuditLogAsync(HttpRequest request, ApplicationUser user, LogAction action)
        {
            _context.Add(new AuditLog
            {
                User = user,
                Action = action,
                Timestamp = DateTime.Now,
                IpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            
            await _context.SaveChangesAsync();
        }
    }
}