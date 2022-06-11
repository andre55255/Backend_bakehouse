using Bakehouse.Core.RepositoriesInterface;
using Bakehouse.Infrastructure.RepositoriesImpl;
using Microsoft.Extensions.DependencyInjection;

namespace Bakehouse.API.Extensions
{
    public static class AddRepositoriesStartup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}