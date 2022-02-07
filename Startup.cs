using ApplicationSecurity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationSecurity.Services;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ApplicationSecurity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordWithHistoryValidator>();
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                
                // https://github.com/aspnet/AspNetIdentity/blob/b7826741279450c58b230ece98bd04b4815beabf/src/Microsoft.AspNet.Identity.Core/PasswordValidator.cs#L58
                options.Password.RequiredLength = 12;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Error/403";
                options.Cookie.Name = "SITConnect";
                options.Cookie.HttpOnly = true;
                
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // 20 min then time out
                // redirects to this page after session time out:
                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true; // time out only when idling
            });
            
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // ensure cookie is validated fast enough:
                //  cookie is invalidated after log out to prevent session hijacking
                //  but ValidationInterval is 30 mins which might be too slow
                //  the below code reduces it to 10 seconds
                options.ValidationInterval = TimeSpan.FromSeconds(10);
            });
            
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            
            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));

            services.AddScoped<EncryptionService>();
            services.AddScoped<AuditLogService>();
            services.AddScoped<PasswordLogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error/500"); // handles unhandled exceptions
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            
            if (env.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
