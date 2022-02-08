using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationSecurity.Data;
using ApplicationSecurity.Middleware;
using ApplicationSecurity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ApplicationSecurity.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [IgnorePasswordExpiry]
    public class LogoutModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly AuditLogService _auditLogService;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, UserManager<ApplicationUser> userManager, AuditLogService auditLogService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _auditLogService = auditLogService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            
            await _signInManager.SignOutAsync();
            
            // invalidate cookie on logout
            await _userManager.UpdateSecurityStampAsync(user);
            
            _logger.LogInformation("User logged out.");
            await _auditLogService.AddAuditLogAsync(Request, user, LogAction.Logout);
            
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
