using Bakehouse.Core.ServicesInterface;
using Bakehouse.Infrastructure.ServicesImpl;
using Microsoft.Extensions.DependencyInjection;

namespace Bakehouse.API.Extensions
{
    public static class AddServicesStartup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IFileManyService, FileManyService>();
            services.AddScoped<IFileUniqueService, FileUniqueService>();
            services.AddScoped<IGenericTypeService, GenericTypeService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}