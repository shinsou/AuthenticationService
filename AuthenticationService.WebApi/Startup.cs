using AuthenticationService.SpaServices;
using AuthenticationService.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Carter;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using Microsoft.AspNetCore.Authentication;

namespace AuthenticationService.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private Serilog.ILogger Logger { get; set; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;

            // From AspNetCore 3 and onwards, ILogger is not being injected into Startup
            this.Logger = Serilog.Log.ForContext<Startup>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.Logger.Information("Add {settings} configuration into services configurations", "Settings");
            services.AddSettings(this.Configuration);

            this.Logger.Information("Add {settings} configuration into services configurations", "Database");
            services.AddDatabase(this.Configuration);

            this.Logger.Information("Add {settings} configuration into services configurations", "CORS");
            services.AddCorsConfigurations(this.Configuration);

            this.Logger.Information("Add {settings} configuration into services configurations", "Custom Services");
            services.AddServices(this.Configuration);

            this.Logger.Information("Add {settings} configuration into services configurations", "SPA");
            services.AddSpa();

            this.Logger.Information("Add {settings} configuration into services configurations", "Carter");
            services.AddCarter(configurator: config => {
                config.WithModelBinder<ModuleHelpers.CustomJsonModelBinder>();

            });

            this.Logger.Information("Configure {settings} options into services configurations", "ForwardedHeaders");
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddAuthorization(this.Configuration);

            this.Logger.Information("Add {settings} configuration into services configurations", "Auth");
            services.AddAuthenticationWithRedis(this.Configuration);

            this.Logger.Information("Add {settings} configuration into services configurations", "DataProtection");
            services.AddDataProtection(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Antiforgery.IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                this.Logger.Information("Current environment is {env}", "Development");

                this.Logger.Information("Configure {env} to be used in application builder", "DeveloperExceptionPage");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                this.Logger.Information("Current environment is {env}", env.EnvironmentName);

                this.Logger.Information("Configure {handler} to be used in application builder", "ExceptionHandler");
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                this.Logger.Information("Configure {middleware} middleware to be used in application builder", "Hsts");
                app.UseHsts();
            }

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "HttpsRedirection");
            app.UseHttpsRedirection();

            //this.Logger.Information("Configure {middleware} middleware to be used in application builder", "StaticFiles");
            //app.UseStaticFiles();

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "SpaStaticFiles");
            app.UseSpaStaticFiles();

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "Routing");
            app.UseRouting();

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "CORS");
            app.UseCors();

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "Authentiction");
            app.UseIdentityServer();

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "Authorization");
            app.UseAuthorization();

            this.Logger.Information("Configure {middleware} middleware to be used in application builder", "Endpoints");
            app.UseEndpoints(endpoints =>
            {
                this.Logger.Information("Configure {middleware} route endpoints to be used in application endpoints middleware", "Carter");
                endpoints.MapCarter();
            });

            //app.Use(next => context =>
            //{
            //    var path = context.Request.Path.Value;

            //    if (
            //string.Equals(path, "/", System.StringComparison.OrdinalIgnoreCase) ||
            //string.Equals(path, "/index.html", System.StringComparison.OrdinalIgnoreCase))
            //    {
            //        // The request token can be sent as a JavaScript-readable cookie, 
            //        // and Angular uses it by default.
            //        var tokens = antiforgery.GetAndStoreTokens(context);
            //        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //            new Microsoft.AspNetCore.Http.CookieOptions() { HttpOnly = false });
            //    }

            //    return next(context);
            //});

            this.Logger.Information("Configure {middleware} middleware into application builder", "SPA");
            app.UseSpa(spa =>
            {
                this.Logger.Information("Configure {option} option for SPA builder, with value {value}", "SourcePath", "ClientApp");
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    this.Logger.Information("Configure {frontendUI} middleware for SPA builder", "SpaDevelopmentServer");
                    spa.UseSpaDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
