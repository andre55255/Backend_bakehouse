using Bakehouse.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bakehouse.API.Extensions
{
    public static class AddDbContextStartup
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString, 
                                 builder => builder.EnableRetryOnFailure());
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);

            return services;
        }
    }
}