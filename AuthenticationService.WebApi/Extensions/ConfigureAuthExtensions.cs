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
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
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

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            services.AddAuthentication(options => {
                        options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.Audience = "https://localhost:5001";
                        options.Authority = "https://localhost:5001";
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters();
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddMultipleOpenIdConnect(authSettings); // allow multiple providers defined in settings => iterate and add all
            
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
            });
            

            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();

            return services;
        }
    }
}
