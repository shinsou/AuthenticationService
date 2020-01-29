using AuthenticationService.Application.Settings;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Utils
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddMultipleOpenIdConnect(this AuthenticationBuilder builder, AuthenticationSettings settings)
        {
            var providers = settings.Providers;

            foreach (var provider in providers)
                builder.AddOpenIdConnect(provider.Schema ?? OpenIdConnectDefaults.AuthenticationScheme, provider.Name, options =>
                {
                    options.SignInScheme = provider.SignInScheme ?? IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = provider.SignOutScheme ?? IdentityServerConstants.SignoutScheme;
                    options.ResponseType = provider.ResponseType; // "code";
                    options.CallbackPath = provider.CallbackPath; // "/callback";
                    options.SignedOutCallbackPath = provider.SignedOutCallbackPath;
                    // Auth0 test id
                    options.ClientId = provider.ClientId;
                    // Auth0 test secret
                    options.ClientSecret = provider.Secret;
                    options.Authority = provider.Authority;

                    var scopes = provider.Scopes; // options.Scope.Add("profile");
                    for (var i = 0; i < scopes.Length; ++i)
                        options.Scope.Add(scopes[i]);

                    options.Events.OnRemoteFailure += context =>
                    {
                        var uri = context.Properties.RedirectUri;
                        var exceptionMessage = context.Failure.Message;
                        var errorType = exceptionMessage.GetSafeStringBetween("error: '", "'");
                        var errorDescription = exceptionMessage.GetSafeStringBetween("error_description: '", "'");

                        context.Response.Redirect(!String.IsNullOrWhiteSpace(errorDescription)
                            ? $"{uri}&remoteError={errorDescription}"
                            : $"{uri}&remoteError=Something went really wrong with remote login. Please contact system administrators");

                        context.HandleResponse();

                        return Task.CompletedTask;
                    };
                });

            return builder;
        }
    }
}
