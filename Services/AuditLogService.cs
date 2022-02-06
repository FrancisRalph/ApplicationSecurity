using System;
using System.Threading.Tasks;
using ApplicationSecurity.Data;

namespace ApplicationSecurity.Services
{
    public class AuditLogService
    {
        private readonly ApplicationDbContext _context;

        public AuditLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAuditLogAsync(ApplicationUser user, LogAction action)
        {
            _context.Add(new AuditLog
            {
                User = user,
                Action = action,
                Timestamp = DateTime.Now
            });
            
            await _context.SaveChangesAsync();
        }
    }
}