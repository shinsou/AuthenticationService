using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers;
using AuthenticationService.WebApi.Extensions;

namespace AuthenticationService.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
            => CreateHostBuilder(args)
                .Build()
                .MigrateApplicationDatabase()
                .MigrateOperationalDatabase()
                .ShowHostAddressInTitle()
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, conf) => {
                    conf.ReadFrom.Configuration(ctx.Configuration);
                    conf.Enrich.With(new ThreadIdEnricher());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
