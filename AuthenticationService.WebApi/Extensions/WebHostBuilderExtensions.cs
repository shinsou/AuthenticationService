using AuthenticationService.Domain;
using AuthenticationService.Persistence.Contexts;
using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Extensions
{
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Will use [<see cref="ApplicationDbContext" />] design factory to apply entity framework migration models pre-generated.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHost MigrateApplicationDatabase(this IHost builder)
        {
            using (var serviceScope = builder.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.AddApplicationMigrate();
            }

            return builder;
        }

        /// <summary>
        /// Will use [<see cref="CustomConfigurationDbContext" />] design factory to apply entity framework migration models pre-generated.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHost MigrateOperationalDatabase(this IHost builder)
        {
            using (var serviceScope = builder.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.AddOperationalMigrate();
            }

            return builder;
        }

        /// <summary>
        /// Apply implementation that will show service hosting address in service task title
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHost ShowHostAddressInTitle(this IHost builder)
        {
            var appName = System.AppDomain.CurrentDomain.FriendlyName;
            var server = builder.Services.GetRequiredService<IServer>();
            var address = server.Features.Get<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>().Addresses.FirstOrDefault();
            Console.Title = $"{appName} - {address}";

            return builder;
        }

        #region Private methods

        private static void AddApplicationMigrate(this IServiceScope serviceScope)
        {
            
            var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDesignTimeDbContextFactory<ApplicationDbContext>>();
            var contextTransactionService = contextFactory.CreateDbContext(null);

            contextTransactionService.Database.Migrate();
            if (!contextTransactionService.Users.Any())
            {
                var passwordHasher = serviceScope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
                var password = passwordHasher.HashPassword(DemoData.DemoUser, DemoData.UserInfo.Password);

                DemoData.DemoUser.PasswordHash = password;

                var userGateway = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var result = userGateway.CreateAsync(DemoData.DemoUser, DemoData.UserInfo.Password).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new Exception("Couldn't create seed user!");
                }

                var user = userGateway.FindByNameAsync(DemoData.DemoUser.UserName).GetAwaiter().GetResult();

                result = userGateway.AddClaimsAsync(user, new System.Security.Claims.Claim[] {
                        new System.Security.Claims.Claim(JwtClaimTypes.Name, DemoData.UserInfo.Name),
                        new System.Security.Claims.Claim(JwtClaimTypes.GivenName, DemoData.UserInfo.GivenName),
                        new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, DemoData.UserInfo.FamilyName),
                        new System.Security.Claims.Claim(JwtClaimTypes.Email, DemoData.UserInfo.Email),
                        new System.Security.Claims.Claim(JwtClaimTypes.EmailVerified, DemoData.UserInfo.EmailVerified)
                    }).GetAwaiter().GetResult();

                if (!result.Succeeded) {
                    var rollbackResult = userGateway.DeleteAsync(user).GetAwaiter().GetResult();
                    
                    if (!rollbackResult.Succeeded)
                        throw new Exception("Failed to rollback seed of demo user, while failed associate demo user claims!");

                    throw new Exception($"Couldn't associate user claims for user {user.UserName}!");
                }

                //contextTransactionService.Users.Add(DemoData.DemoUser);
                //contextTransactionService.SaveChangesAsync().GetAwaiter().GetResult();

                //var user = contextTransactionService.Users.FirstOrDefault();
                //contextTransactionService.UserClaims.AddRange(
                //    new System.Security.Claims.Claim[] {
                //        new System.Security.Claims.Claim(JwtClaimTypes.Name, DemoData.UserInfo.Name),
                //        new System.Security.Claims.Claim(JwtClaimTypes.GivenName, DemoData.UserInfo.GivenName),
                //        new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, DemoData.UserInfo.FamilyName),
                //        new System.Security.Claims.Claim(JwtClaimTypes.Email, DemoData.UserInfo.Email),
                //        new System.Security.Claims.Claim(JwtClaimTypes.EmailVerified, DemoData.UserInfo.EmailVerified)
                //    }.Select(x => new IdentityUserClaim<Guid> {
                //        ClaimType = x.Type,
                //        ClaimValue = x.Value,
                //        UserId = user.Id
                //    })
                //);

                //contextTransactionService.SaveChangesAsync().GetAwaiter().GetResult();
            }
        }

        private static void AddOperationalMigrate(this IServiceScope serviceScope)
        {
            var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDesignTimeDbContextFactory<CustomConfigurationDbContext>>();
            var contextTransactionService = contextFactory.CreateDbContext(null);

            contextTransactionService.Database.Migrate();
        }

        #endregion
    }
}
