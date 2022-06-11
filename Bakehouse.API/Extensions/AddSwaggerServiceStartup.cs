using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Bakehouse.API.Extensions
{
    public static class AddSwaggerServiceStartup
    {
        public static IServiceCollection AddSwaggerGeneration(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Bakehouse.API",
                    Version = "v1",
                    Description = "API para projeto de gestão de padaria"
                });

                // Habilitando Swagger para ler comentários de documentação de controller
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //opt.IncludeXmlComments(xmlPath);

                // Habilitando Swagger para autenticação Jwt Bearer
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization. Bearer token.<br/>
                    <br/>Entre com 'Bearer'[space] e depois digite o valor do token obtido no login.
                    <br/>Exemplo: \""'Bearer 12345abcdef\"""
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            return services;
        }
    }
}