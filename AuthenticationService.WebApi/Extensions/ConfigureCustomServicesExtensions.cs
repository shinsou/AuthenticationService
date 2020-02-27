using AuthenticationService.Application.Factories;
using AuthenticationService.Application.Gateways;
using AuthenticationService.Application.Repositories;
using AuthenticationService.Core;
using AuthenticationService.Domain;
using AuthenticationService.Infrastructure.Gateways;
using AuthenticationService.Persistence.Contexts;
using AuthenticationService.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Extensions
{
    public static class ConfigureCustomServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILdapGateway<User>, LdapGateway>();
            services.AddScoped<IAuditRepository<UserAudit>, AuditRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<UserStore<User, Role, ApplicationDbContext, Guid>, UserRepository>();
            services.AddScoped<UserManager<User>, UserGateway>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, Guid>, RoleRepository>();
            services.AddScoped<RoleManager<Role>, RoleGateway>();
            services.AddScoped<SignInManager<User>, SignInGateway>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationUserClaimsPrincipalFactory>();

            return services;
        }
    }
}
