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

        private void SetErrorMessage()
        {
            switch (StatusCode)
            {
                case 403:
                    ErrorMessage = "You are not authorized to access this page.";
                    break;
                case 500:
                    ErrorMessage = "Something went wrong when processing your request.";
                    break;
                default:
                    StatusCode = 404;
                    ErrorMessage = "We could not find what you were looking for.";
                    break;
            }
        }

        public void OnGet()
        {
            SetErrorMessage();
        }

        public void OnPost()
        {
            SetErrorMessage();
        }
    }
}
