using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationSecurity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;

namespace ApplicationSecurity.Middleware
{
    public class IgnorePasswordExpiryAttribute : Attribute {}

    public class PasswordExpiryMiddleware
    {
        private const int MaximumAgeInMinutes = 1;
        private readonly RequestDelegate _next;

        public PasswordExpiryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var hasIgnoreAttribute = context.Features.Get<IEndpointFeature>().Endpoint?.Metadata
                .Any(m => m is IgnorePasswordExpiryAttribute);
            
            if (hasIgnoreAttribute != true)
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user != null)
                {
                    var timeSpanSinceLastChanged = DateTime.UtcNow - user.PasswordLastChanged;
                    if (timeSpanSinceLastChanged > TimeSpan.FromMinutes(MaximumAgeInMinutes))
                    {
                        context.Response.Redirect("/Identity/Account/Manage/ChangePassword?expired=true");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

    public static class PasswordExpiryMiddlewareExtensions
    {
        public static IApplicationBuilder UsePasswordExpiryMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PasswordExpiryMiddleware>();
        }
    }
}