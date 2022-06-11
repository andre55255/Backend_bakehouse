using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bakehouse.API.Extensions
{
    public static class AddConfigAuthJwtStartup
    {
        public static IServiceCollection AddAuthJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = "JwtBearer";
                opt.DefaultChallengeScheme = "JwtBearer";
            })
           .AddJwtBearer("JwtBearer", opt =>
           {
               opt.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey =
                       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                   ClockSkew = TimeSpan.FromMinutes(30),
                   ValidIssuer = configuration["JWT:ValidIssuer"],
                   ValidAudience = configuration["JWT:ValidAudience"]
               };

               opt.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   }
               };
           });

            return services;
        }
    }
}