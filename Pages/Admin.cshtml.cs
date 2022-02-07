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

        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostError404Async()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            return NotFound();
        }
        
        public async Task<IActionResult> OnPostError403Async()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            return Forbid();
        }
        
        public async Task<IActionResult> OnPostError500Async()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            throw new Exception("Intentionally triggered error");
        }
        
        public async Task<IActionResult> OnPostError400Async()
        {
            if (!await IsUserAdminAsync())
            {
                return Forbid();
            }

            return BadRequest();
        }
    }
}
