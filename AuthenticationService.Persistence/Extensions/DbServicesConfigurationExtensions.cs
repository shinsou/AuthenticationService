using AuthenticationService.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Persistence.Extensions
{
    public static class DbServicesConfigurationExtensions
    {
        /// <summary>
        /// Add DbContext with defaults to service collection
        /// </summary>
        /// <typeparam name="TContext">EntityFramework DbContext class</typeparam>
        /// <param name="services">API Service collection</param>
        /// <param name="contextConnectionType">name of used database type, ignore case (SqlServer, Sqlite, Postgre)</param>
        /// <param name="connectionString">Database connection string</param>
        /// <param name="maxPoolSize">Maximum connections in pool</param>
        /// <returns></returns>
        public static IServiceCollection AddDbContextPoolWithDefaults<TContext>(
            this IServiceCollection services,
            string contextConnectionType,
            string connectionString,
            int maxPoolSize = 30) where TContext : DbContext
            => AddDbContextPoolWithDefaults<TContext>(services, ResolveContextConnectionType(contextConnectionType), connectionString, maxPoolSize);

        /// <summary>
        /// Add DbContext with defaults to service collection
        /// </summary>
        /// <typeparam name="TContext">EntityFramework DbContext class</typeparam>
        /// <param name="services">API Service collection</param>
        /// <param name="contextConnectionType">Enum type of used database type (SqlServer, Sqlite, Postgre)</param>
        /// <param name="connectionString">Database connection string</param>
        /// <param name="maxPoolSize">Maximum connections in pool
        /// Note! 
        /// Limiting pool size smaller than required can cause abnormal behaviour and performance issues</param>
        /// <returns></returns>
        public static IServiceCollection AddDbContextPoolWithDefaults<TContext>(
            this IServiceCollection services,
            DbConnectionTypes contextConnectionType,
            string connectionString,
            int maxPoolSize = 30) where TContext : DbContext
        {
            switch (contextConnectionType)
            {
                case DbConnectionTypes.SqlServer:
                    services.AddEntityFrameworkSqlServer().AddDbContextPool<TContext>((serviceProvider, options) =>
                    {
                        options.UseSqlServer(connectionString);
                        options.UseInternalServiceProvider(serviceProvider);
                    }, maxPoolSize);
                    break;
                case DbConnectionTypes.SqlLite:
                    services.AddEntityFrameworkSqlite().AddDbContextPool<TContext>((serviceProvider, options) =>
                    {
                        options.UseSqlite(connectionString);
                        options.UseInternalServiceProvider(serviceProvider);
                    }, maxPoolSize);
                    break;
                case DbConnectionTypes.PostgreSql:
                    services.AddEntityFrameworkNpgsql().AddDbContextPool<TContext>((serviceProvider, options) =>
                    {
                        options.UseNpgsql(connectionString);
                        options.UseInternalServiceProvider(serviceProvider);
                    }, maxPoolSize);
                    break;
            }

            return services;
        }

        public static string[] GetContextConnectionTypeNames()
        {
            return Enum.GetNames(typeof(DbConnectionTypes));
        }

        private static DbConnectionTypes ResolveContextConnectionType(string name)
        {
            return (DbConnectionTypes)Enum.Parse(typeof(DbConnectionTypes), name, true);
        }
    }
}
