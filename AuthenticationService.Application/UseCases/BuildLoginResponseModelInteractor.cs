using AuthenticationService.Core;
using AuthenticationService.Domain;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.UseCases
{
    public class BuildLoginResponseModelInteractor : IInteractor<LoginResponse, string>
    {
        #region Members

        private HttpContext HttpContext { get; }
        private IIdentityServerInteractionService InteractionService { get; }
        private IAuthenticationSchemeProvider SchemeProvider { get; }
        private IClientStore ClientStore { get; }
        private ILogger<BuildLoginResponseModelInteractor> Logger { get; }

        #endregion
        public BuildLoginResponseModelInteractor(
            HttpContext httpContext,
            IIdentityServerInteractionService interactionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IClientStore clientStore,
            ILoggerFactory loggerFactory)
        {
            this.HttpContext = httpContext;

            this.InteractionService = interactionService;
            this.ClientStore = clientStore;
            this.SchemeProvider = authenticationSchemeProvider;

            this.Logger = loggerFactory.CreateLogger<BuildLoginResponseModelInteractor>();
        }
        public async Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, LoginResponse> presenter, string returnUrl)
        {
            var authRequestContext = await this.InteractionService.GetAuthorizationContextAsync(returnUrl);

            if (authRequestContext?.IdP != null)
            {
                return presenter.Process(new LoginResponse
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = authRequestContext?.LoginHint,
                    ExternalProviders = new ExternalProvider[] { new ExternalProvider { AuthenticationScheme = authRequestContext.IdP } }
                });
            }

            var schemes = await this.SchemeProvider.GetAllSchemesAsync();
            var providers = schemes
                .Where(scheme
                    => scheme.DisplayName != null
                    || scheme.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase)
                )
                .Select(scheme => new ExternalProvider {
                    DisplayName = scheme.DisplayName,
                    AuthenticationScheme = scheme.Name
                });

            var allowLocal = true;

            if (authRequestContext?.ClientId != null)
            {
                var client = await this.ClientStore.FindEnabledClientByIdAsync(authRequestContext.ClientId);

                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => !client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
                    }
                }
            }

            return presenter.Process(new LoginResponse
            {
                RememberLogin = AccountOptions.RememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = authRequestContext?.LoginHint,
                ExternalProviders = providers.ToArray()
            });
        }
    }
}
