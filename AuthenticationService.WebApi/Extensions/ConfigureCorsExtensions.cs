using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Extensions
{
    public static class ConfigureCorsExtensions
    {
        public static IServiceCollection AddCorsConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            //var settings = new CorsSettings(configuration);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                        builder.WithOrigins("https://localhost:5001",
                                            "http://localhost:5000",
                                            "http://localhost:8080")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
