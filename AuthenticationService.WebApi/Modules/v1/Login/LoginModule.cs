using AuthenticationService.Application.UseCases;
using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using FluentValidation;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.Login
{
    public class LoginModule : VersionModule
    {
        private ILogger<LoginModule> Logger { get; }
        public LoginModule(
            IIdentityServerInteractionService interactionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IClientStore clientStore,
            ILoggerFactory loggerFactory) : base("/login")
        {
            this.Logger = loggerFactory.CreateLogger<LoginModule>();

            Get("/", async (req, res) =>
            {
                this.Logger.LogDebug("Do lookup in request query for {key}", "ReturnUrl");
                var returnUrl = req.Query.As<string>("returnurl");
                if (!String.IsNullOrWhiteSpace(returnUrl))
                    this.Logger.LogTrace("Found value {value} for key {key}", returnUrl, "ReturnUrl");

                if (req.HttpContext.User.IsAuthenticated()) {
                    this.Logger.LogDebug($"Session stated as already authenticated for user [{req.HttpContext.User.Identity.Name}]");

                    res.Redirect(String.IsNullOrWhiteSpace(returnUrl)
                        ? "/"
                        : returnUrl);
                    return;
                }

                this.Logger.LogDebug("Create {interactor} interactor", "BuildLoginResponseModel");
                var interactor = new BuildLoginDefaultsInteractor(
                    req.HttpContext,
                    interactionService,
                    authenticationSchemeProvider,
                    clientStore,
                    loggerFactory);

                this.Logger.LogDebug("Run {interactor}'s handler to build {model}", "BuildLoginResponseModelInteractor", "LoginResponse");
                var viewModel = await interactor.HandleAsync(
                    new Presenter(),
                    returnUrl);

                await res.Negotiate(viewModel);
            });

            Post("/", async (req, res) =>
            {
                var test1 = await req.Bind<ViewModel>();
                res.StatusCode = 401;
                //await res.Negotiate(null);
            });
        }
    }
}
