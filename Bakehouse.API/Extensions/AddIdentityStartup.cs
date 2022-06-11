using Bakehouse.Core.Entities;
using Bakehouse.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Bakehouse.API.Extensions
{
    public static class AddIdentityStartup
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(opt =>
                       opt.SignIn.RequireConfirmedEmail = true
                   )
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders()
                   .AddTokenProvider("JwtRefreshToken",
                                     typeof(DataProtectorTokenProvider<ApplicationUser>));

            // Config senhas de usuarios aceitas
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 3;
            });

            return services;
        }
    }
}