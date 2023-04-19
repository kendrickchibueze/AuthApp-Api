using AuthApp_Api.Models;
using AuthApp_Api.Services;
using AuthApp_Api.Services.Interface;
using Microsoft.AspNetCore.Mvc.Routing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AuthApp_Api.Extensions
{
    public static  class Middleware
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<IUserTwoFactorTokenProvider<AppUser>, MyTokenProvider>();
            return services;
        }


        public static void ConfigureEmail(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }

}
 
