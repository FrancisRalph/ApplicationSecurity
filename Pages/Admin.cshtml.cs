using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationSecurity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ApplicationSecurity.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public PrivacyModel(ILogger<PrivacyModel> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        private async Task<bool> IsUserAdminAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.NormalizedUserName == "ADMIN@GMAIL.COM";
        }

        public async Task<IActionResult> OnGet()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPost()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            return Page();
        }
    }
}
