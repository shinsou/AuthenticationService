using AuthenticationService.Application.UseCases;
using AuthenticationService.Domain;
using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using FluentValidation;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEventService events,
            IAntiforgery antiForgery,
            IClientStore clientStore,
            IIdentityServerInteractionService interactionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
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

                    var test = req.HttpContext.Session;

                    await res.Negotiate(new ViewModel
                    {
                        Username = req.HttpContext.User.Identity.Name,
                        ReturnUrl = returnUrl,
                        RedirectUri = String.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl
                    });
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

                var antiforgeryToken = antiForgery.GetAndStoreTokens(req.HttpContext);
                viewModel.RequestVerificationToken = antiforgeryToken.RequestToken;

                await res.Negotiate(viewModel);
            });

            Post("/", async (req, res) =>
            {
                try {
                    await antiForgery.ValidateRequestAsync(req.HttpContext);

                    var validationResult = await req.BindAndValidate<ViewModel>();
                    if (!validationResult.ValidationResult.IsValid){
                        var errorMessages = String.Join("; ", validationResult.ValidationResult.Errors.Select(e => e.ErrorMessage));

                        res.StatusCode = 400;
                        await res.Negotiate(new ViewModel {
                            Message =  errorMessages});

                        return;
                    }
                    
                    var loginRequest = validationResult.Data;

                    var interactor = new CreateUserLoginInteractor(
                        userManager,
                        signInManager,
                        events,
                        clientStore,
                        interactionService,
                        authenticationSchemeProvider,
                        loggerFactory);

                    var viewModel = await interactor.HandleAsync(
                        new Presenter(),
                        loginRequest.Username,
                        loginRequest.Password,
                        loginRequest.RedirectUri);

                    // set user identity claims into response, if success
                    if (viewModel.Message?.Length == 0)
                        viewModel.Identity = req.HttpContext.User;

                    await res.Negotiate(viewModel);
                } catch (AntiforgeryValidationException avex) {
                    this.Logger.LogError(avex, avex.Message);
                    res.StatusCode = 400;
                } catch (Exception ex) {
                    this.Logger.LogError(ex, ex.Message);
                    res.StatusCode = 500;
                }
            });
        }
    }
}
