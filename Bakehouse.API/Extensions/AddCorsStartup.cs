using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bakehouse.API.Extensions
{
    public static class AddCorsStartup
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(
                            new string[]
                            {
                                    configuration["SSLURL"] + configuration["HostedURL"],
                                    configuration["SSLURL"] + "www." + configuration["HostedURL"]
                            })
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}