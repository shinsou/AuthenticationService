using AuthenticationService.Application.Settings;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Extensions
{
    public static class ConfigureDataProtectionExtensions
    {
        public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new RedisSettings(configuration);

            var connectionMultiplexer = ConnectionMultiplexer.Connect(settings.GetConnectionString());
            services.AddDataProtection()
                    .SetApplicationName(settings.AppName)
                    .PersistKeysToStackExchangeRedis(connectionMultiplexer, settings.DataProtectionKeyName);

            services.AddAntiforgery();
            //services.AddAntiforgery(opts => opts.HeaderName = "X-XSRF-Token");

            return services;
        }
    }
}
