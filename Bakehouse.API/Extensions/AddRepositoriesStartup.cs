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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IGenericTypeRepository, GenericTypeRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();

            return services;
        }
    }
}