using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationSecurity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationSecurity.Services
{
    public class PasswordLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _hasher;

        public PasswordLogService(ApplicationDbContext context, IPasswordHasher<ApplicationUser> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task LogPasswordAsync(ApplicationUser user)
        {
            _context.PasswordLogs.Add(new PasswordLog
            {
                User = user,
                PasswordHash = user.PasswordHash,
                CreatedAt = DateTime.Now,
            });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserPasswordExceededMinimumAgeAsync(ApplicationUser user, TimeSpan minimumAge)
        {
            var lastChanged = await _context.PasswordLogs
                .Where(p => p.User.Id == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => (DateTime?) p.CreatedAt)
                .FirstOrDefaultAsync();

            var timeSpanSinceLastChanged = DateTime.Now - (lastChanged ?? DateTime.MinValue);
            
            return timeSpanSinceLastChanged > minimumAge;
        }

        public async Task<bool> IsPasswordAlreadyUsedAsync(ApplicationUser user, string passwordPlaintext,
            int historyCount)
        {
            return (await _context.PasswordLogs
                    .Where(p => p.User.Id == user.Id)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => p.PasswordHash)
                    .Take(historyCount)
                    .Select(hash =>
                        _hasher.VerifyHashedPassword(user, hash, passwordPlaintext)
                        == PasswordVerificationResult.Success)
                    .ToListAsync()
                ).Any(b => b);
        }
    }
}