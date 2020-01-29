using AuthenticationService.Application.Settings;
using AuthenticationService.Domain;
using AuthenticationService.WebApi.Utils;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Extensions
{
    public static class ConfigureAuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = new DatabaseSettings(configuration);
            var redisSettings = new RedisSettings(configuration);
            var authSettings = new AuthenticationSettings(configuration);
            //services.AddSession(options =>
            //{
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            //    options.Cookie.Name = redisSettings.AppName;
            //    options.Cookie.HttpOnly = true;
            //});
            services.AddIdentityServer(opt =>
                    {
                        opt.Events.RaiseErrorEvents = true;
                        opt.Events.RaiseInformationEvents = true;
                        opt.Events.RaiseFailureEvents = true;
                        opt.Events.RaiseSuccessEvents = true;
                    })
                    //.AddEnvironmentBasedSigningCredential(environment, configuration)
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseNpgsql(dbSettings.ConnectionString,
                                sql => sql.MigrationsAssembly(dbSettings.DatabaseInstanceName));
                    })
                    .AddOperationalStore(options =>
                    {
                        options.RedisConnectionString = redisSettings.ConnectionString;
                        options.KeyPrefix = "ops_";
                    })
                    .AddRedisCaching(options =>
                    {
                        options.RedisConnectionString = redisSettings.ConnectionString;
                        options.KeyPrefix = "redis_";
                    })
                    .AddAspNetIdentity<User>()
                    ;

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddMultipleOpenIdConnect(authSettings); // allow multiple providers defined in settings => iterate and add all

            return services;
        }
    }
}
