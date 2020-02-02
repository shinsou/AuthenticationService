using AuthenticationService.Persistence.Contexts;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Persistence.Factories
{
    public class ConfigureConfigurationDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<CustomConfigurationDbContext>
    {
        public CustomConfigurationDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment") ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<CustomConfigurationDbContext>();
            var connectionString = configuration.GetValue<string>("Database:ConnectionString");

            builder.UseNpgsql(connectionString);

            return new CustomConfigurationDbContext(builder.Options, new IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions());
        }
    }
}
