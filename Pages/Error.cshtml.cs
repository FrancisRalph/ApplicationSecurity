using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationSecurity.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        
        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (StatusCode == 0)
            {
                StatusCode = 404;
            }
            
            switch (StatusCode)
            {
                case 404:
                    ErrorMessage = "Not found";
                    break;
                case 403:
                    ErrorMessage = "Forbidden";
                    break;
            }
        }
    }
}
