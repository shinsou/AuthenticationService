using AuthenticationService.Application.Settings;
using AuthenticationService.Domain;
using AuthenticationService.WebApi.Utils;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
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
        public static IServiceCollection AddAuthenticationWithRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = new DatabaseSettings(configuration);
            var redisSettings = new RedisSettings(configuration);
            var authSettings = new AuthenticationSettings(configuration);

            services.AddSession(options =>
            {
                options.Cookie.Name = redisSettings.AppName;

                // enforce use of Https (applies also to local/dev)
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // to comfort chrome security changes
                options.Cookie.SameSite = SameSiteMode.Lax;

                // this will elevate cookie to HttpOnly => cookie will not be visible for Javascript
                options.Cookie.HttpOnly = true;
            });

            services.AddIdentityServer(opt =>
                    {
                        opt.Events.RaiseErrorEvents = true;
                        opt.Events.RaiseInformationEvents = true;
                        opt.Events.RaiseFailureEvents = true;
                        opt.Events.RaiseSuccessEvents = true;
                    })
                    //.AddEnvironmentBasedSigningCredential(environment, configuration)
                    .AddConfigurationStore(options => SetupConfigurationStoreOptions(options, dbSettings))
                    .AddOperationalStore(options => {
                        options.RedisConnectionString = redisSettings.GetConnectionString();
                        options.KeyPrefix = "ops_";
                    })
                    .AddRedisCaching(options => SetupRedisCachingOptions(options, redisSettings))
                    .AddAspNetIdentity<User>()
                    ;

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                // commented code below, settings => unnecessary; and will cause problems if defined in this way, such as Cookie authentication not applied at all!
            //services.AddAuthentication(options => {
            //            options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            //            options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            //            options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            //            options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            //        })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Audience = "https://localhost:5001";
                        options.Authority = "https://localhost:5001";
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters();
                    })
                    .AddMultipleOpenIdConnect(authSettings); // allow multiple providers defined in settings => iterate and add all
            
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";

                options.Events.OnRedirectToLogin = context =>
                {
                    //context.Response.StatusCode == StatusCodes.Status401Unauthorized
                    return Task.CompletedTask;
                };

                options.ExpireTimeSpan = TimeSpan.FromMinutes(authSettings.SessionExpirationMinutes);
            });
            

            return services;
        }

        #region Private methods

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();

            return services;
        }

        private static void SetupConfigurationStoreOptions(IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions options, DatabaseSettings dbSettings)
        {
            options.ConfigureDbContext = builder =>
                builder.UseNpgsql(dbSettings.ConnectionString,
                    sql => sql.MigrationsAssembly(dbSettings.DatabaseInstanceName));
        }

        private static void SetupRedisCachingOptions(IdentityServer4.Contrib.RedisStore.RedisCacheOptions options, RedisSettings redisSettings)
        {
            options.RedisConnectionString = redisSettings.GetConnectionString();
            options.KeyPrefix = "redis_";
        }

        #endregion
    }
}
