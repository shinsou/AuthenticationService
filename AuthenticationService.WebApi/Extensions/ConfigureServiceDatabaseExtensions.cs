using AuthenticationService.Application.Settings;
using AuthenticationService.Domain;
using AuthenticationService.Infrastructure.Gateways;
using AuthenticationService.Persistence.Contexts;
using AuthenticationService.Persistence.Extensions;
using AuthenticationService.Persistence.Factories;
using AuthenticationService.Persistence.Helpers;
using AuthenticationService.Persistence.Repositories;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Extensions
{
    public static class ConfigureServiceDatabaseExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new DatabaseSettings(configuration);

            // configure application context (users, roles, claims, etc.)
            services.AddDbContextPoolWithDefaults<ApplicationDbContext>(DbConnectionTypes.PostgreSql, settings.ConnectionString);

            // configure identity server basic elements (client, resource, etc.)
            services.AddDbContextPoolWithDefaults<ConfigurationDbContext>(DbConnectionTypes.PostgreSql, settings.ConnectionString);

            // implement DesignTimeFactory for migration builder
            services.AddSingleton<IDesignTimeDbContextFactory<ApplicationDbContext>, ConfigureApplicationDbContextFactory>();
            services.AddSingleton<IDesignTimeDbContextFactory<CustomConfigurationDbContext>, ConfigureConfigurationDbContextFactory>();

            services.AddIdentity<User, Role>()
                    .AddUserStore<UserRepository>()
                    .AddUserManager<UserGateway>()
                    .AddRoleStore<RoleRepository>()
                    .AddRoleManager<RoleGateway>()
                    .AddSignInManager<SignInGateway>()
                    .AddDefaultTokenProviders();
            
            return services;
        }
    }
}
