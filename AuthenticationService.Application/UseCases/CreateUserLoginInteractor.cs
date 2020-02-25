using AuthenticationService.Core;
using AuthenticationService.Domain;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.UseCases
{
    public class CreateUserLoginInteractor : IInteractor<LoginResponse, string, string, string, bool>
    {
        UserManager<User> UserGateway { get; }
        SignInManager<User> SignInGateway { get; }
        IEventService Events { get; }
        IClientStore ClientStore { get; }
        IIdentityServerInteractionService InteractionService { get; }
        IAuthenticationSchemeProvider AuthenticationSchemeProvider { get; }
        ILogger<CreateUserLoginInteractor> Logger { get; }

        public CreateUserLoginInteractor(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEventService events,
            IClientStore clientStore,
            IIdentityServerInteractionService interactionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            ILoggerFactory loggerFactory)
        {
            this.UserGateway = userManager;
            this.SignInGateway = signInManager;
            this.Events = events;
            this.ClientStore = clientStore;
            this.InteractionService = interactionService;
            this.AuthenticationSchemeProvider = authenticationSchemeProvider;

            this.Logger = loggerFactory.CreateLogger<CreateUserLoginInteractor>();
        }

        public async Task<TViewModel> HandleAsync<TViewModel>(
            IPresenter<TViewModel, LoginResponse> presenter, 
            string username, 
            string password, 
            string returnUrl, 
            bool rememberLogin = false)
        {
            this.Logger.LogDebug("Validating user credentials against {gateway}", "SignInGateway");
            var result = await this.SignInGateway.PasswordSignInAsync(
                username, 
                password, 
                rememberLogin, 
                lockoutOnFailure: true);

            if (result.Succeeded)
                return await HandleSuccessResultAsync(presenter, username, returnUrl);

            this.Logger.LogWarning($"Login was unsuccessful");

            if (result.IsLockedOut)
                return await HandleLockedOutResultAsync(presenter, username);

            if (result.IsNotAllowed)
                return await HandleLoginNotAllowedResultAsync(presenter, username);

            return await HandleInvalidLoginResultAsync(presenter, username);
        }

        #region Private methods

        private async Task<TViewModel> HandleSuccessResultAsync<TViewModel>(IPresenter<TViewModel, LoginResponse> presenter, string username, string returnUrl)
        {
            this.Logger.LogDebug($"Login successful");

            this.Logger.LogDebug($"Retrieving user object in safe manner");
            var user = await this.UserGateway.FindByNameAsync(username);

            this.Logger.LogDebug($"Raising LoginSuccess event for user");
            await this.Events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));

            this.Logger.LogTrace($"Make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a clients return url");
            this.Logger.LogTrace($"The IsLocalUrl check is only necessary if you want to support additional local pages, otherwise IsValidReturnUrl is more strict");
            this.Logger.LogDebug($"Validating if return url is valid, with client configured url or if it's a local url");
            //if (this.InteractionService.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl)) // url.IsLocalUrl ext. doesn't exist #TODO: Create it
            if (this.InteractionService.IsValidReturnUrl(returnUrl))
            {
                Logger.LogDebug($"Return url is valid. Redirecting login request to specified return url [{returnUrl}]");
                return presenter.Process(new LoginResponse
                {
                    RedirectUri = returnUrl
                });
            }

            Logger.LogDebug($"Redirecting user login request to site root");
            return presenter.Process(new LoginResponse
            {
                RedirectUri = "/"
            });
        }

        private async Task<TViewModel> HandleLockedOutResultAsync<TViewModel>(IPresenter<TViewModel, LoginResponse> presenter, string username)
        {
            Logger.LogWarning($"Account is locked out");
            await this.Events.RaiseAsync(new UserLoginFailureEvent(username, "account locked"));

            return presenter.Process(new LoginResponse
            {
                Message = "Account is locked, please contact system administrator!"
            });
        }

        private async Task<TViewModel> HandleLoginNotAllowedResultAsync<TViewModel>(IPresenter<TViewModel, LoginResponse> presenter, string username)
        {
            this.Logger.LogWarning($"Login method attempted is not allowed for target account");

            this.Logger.LogDebug($"Retrieving user object in safe manner");
            var user = await this.UserGateway.FindByNameAsync(username);

            this.Logger.LogDebug($"Check if user is required to change its password");
            if (user.RequireChangePassword.HasValue && user.RequireChangePassword.Value)
            {
                this.Logger.LogDebug($"User needs to change the password. Redirecting to corresponding view.");
                // redirect user to change password view
                return presenter.Process(new LoginResponse
                {
                    RedirectUri = $"ResetPassword/FirstTime?id={user.Id}"
                });
            }

            this.Logger.LogDebug($"No password change required. Login attempt failed with something else. Raise an event");
            // apparently user password change was not required, then the failure reason is something else
            await this.Events.RaiseAsync(new UserLoginFailureEvent(username, "invalid credentials"));

            this.Logger.LogDebug($"Add error into model response. Invalid credentials.");
            return presenter.Process(new LoginResponse
            {
                Message = "Invalid credentials!" // do not expose account, in case if this was spoofing
            });
        }

        private async Task<TViewModel> HandleInvalidLoginResultAsync<TViewModel>(IPresenter<TViewModel, LoginResponse> presenter, string username)
        {
            Logger.LogDebug($"Login failure state not defined. User doesn't exist or user credentials are invalid. Raise an event");
            await this.Events.RaiseAsync(new UserLoginFailureEvent(username, "invalid credentials"));

            Logger.LogDebug($"Add error into model response. Invalid credentials.");
            return presenter.Process(new LoginResponse
            {
                Message = "Invalid credentials!"
            });
        }

        #endregion
    }
}
