using Bakehouse.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Bakehouse.API
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
            // Add Controllers
            services.AddControllers();

            // Config Swagger
            services.AddSwaggerGeneration();

            // Config de cors
            services.AddCors(Configuration);

            // Contexto de comunicacao com banco de dados
            services.AddDbContext(Configuration.GetConnectionString("EntityConnection"));

            // Config Identity
            services.AddIdentity();

            // Config auth bearer token Jwt
            services.AddAuthJwt(Configuration);

            // Config de auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Config de injecao de repositories
            services.AddRepositories();

            // Config de injecao de dependencias de services
            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bakehouse.API v1"));

            app.UseHttpContext();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Directory")),
                RequestPath = new PathString("/static")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Directory")),
                RequestPath = new PathString("/static")
            });

            app.UseCors("ClientPermission");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
