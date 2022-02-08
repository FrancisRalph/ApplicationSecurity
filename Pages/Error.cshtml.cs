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
            ErrorMessage = StatusCode switch
            {
                400 => "Your browser sent a request we could not understand.",
                403 => "You are not authorized to access this page.",
                500 => "Something went wrong when processing your request.",
                _ => "We could not find what you were looking for."
            };
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
