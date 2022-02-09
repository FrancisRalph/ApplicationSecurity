using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationSecurity.Data;
using Microsoft.AspNetCore.Identity;

namespace ApplicationSecurity.Services
{
    public class PasswordWithHistoryValidator : PasswordValidator<ApplicationUser>
    {
        private const int HistoryCount = 2;
        private const int MinimumAgeInSeconds = 30;
        private readonly PasswordLogService _passwordLogService;

        public PasswordWithHistoryValidator(PasswordLogService passwordLogService, IdentityErrorDescriber errors = null)
            : base(errors)
        {
            _passwordLogService = passwordLogService;
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager,
            ApplicationUser user, string password)
        {
            var result = await base.ValidateAsync(manager, user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            var hasUserPasswordExceededMinimumAge = PasswordLogService.HasUserPasswordExceededMinimumAge(
                user,
                TimeSpan.FromSeconds(MinimumAgeInSeconds)
            );

            if (!hasUserPasswordExceededMinimumAge)
            {
                return IdentityResult.Failed( new IdentityError
                {
                    Code = "PasswordHasNotExceededMinimumAge",
                    Description = $"Please wait {MinimumAgeInSeconds} second(s) before you change your password again"
                });
            }

            var isPasswordAlreadyUsed =
                await _passwordLogService.IsPasswordAlreadyUsedAsync(user, password, HistoryCount);

            if (isPasswordAlreadyUsed)
            {
                return IdentityResult.Failed( new IdentityError
                {
                    Code = "PasswordAlreadyUsed",
                    Description = "Password already used before. Please use a different password"
                });
            }
            
            return IdentityResult.Success;
        }
    }
}