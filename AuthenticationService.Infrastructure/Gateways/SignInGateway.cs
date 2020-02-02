using AuthenticationService.Application.Gateways;
using AuthenticationService.Application.Repositories;
using AuthenticationService.Domain;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Gateways
{
    public class SignInGateway : SignInManager<User>
    {
        private const string LoginProviderKey = "LoginProvider";
        private const string XsrfKey = "XsrfId";

        new private ILogger<SignInGateway> Logger { get; }
        private ILdapGateway<User> LdapGateway { get; }
        private IAuditRepository<UserAudit> AuditRepository { get; }
        private IHttpContextAccessor ContextAccessor { get; }

        public SignInGateway(UserManager<User> userGateway,
                             IHttpContextAccessor contextAccessor,
                             IUserClaimsPrincipalFactory<User> claimsFactory,
                             IOptions<IdentityOptions> optionsAccessor,
                             ILoggerFactory loggerFactory,
                             IAuthenticationSchemeProvider schemes,
                             IUserConfirmation<User> userConfirmation,
                             IAuditRepository<UserAudit> auditRepository,
                             ILdapGateway<User> ldapGateway
            ) : base(userGateway, contextAccessor, claimsFactory, optionsAccessor, loggerFactory.CreateLogger<SignInManager<User>>(), schemes, userConfirmation)
        {
            this.Logger = loggerFactory.CreateLogger<SignInGateway>();
            this.LdapGateway = ldapGateway;
            this.AuditRepository = auditRepository;
            this.ContextAccessor = contextAccessor;
        }
        
        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool shouldLockout)
        {
            var user = await this.UserManager.FindByNameAsync(userName);
            var result = await this.PasswordSignInAsync(user, password, rememberMe, shouldLockout);

            if (user != null)
                await CreateAuditReport(result, user);

            return result;
        }

        public override async Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            if (user == null)
                return SignInResult.Failed;

            var actualUser = await this.UserManager.FindByIdAsync(user.Id.ToString());
            var claims = await this.UserManager.GetClaimsAsync(user);

            if ((actualUser.IsEnabled.HasValue && !actualUser.IsEnabled.Value) || !actualUser.IsEnabled.HasValue)
            {
                return SignInResult.LockedOut;
            }

            if (actualUser.RequireChangePassword.HasValue && actualUser.RequireChangePassword.Value)
            {
                return SignInResult.NotAllowed;
            }

            return await base.PasswordSignInAsync(actualUser, password, isPersistent, lockoutOnFailure);
        }

        public override async Task SignOutAsync()
        {
            await base.SignOutAsync();

            var user = await this.UserManager.FindByIdAsync(this.UserManager.GetUserId(this.ContextAccessor.HttpContext.User));
            if (user != null)
                await CreateAuditReport(null, user);
        }

        public override async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            var auth = await Context.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            //var auth = await Context.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var items = auth?.Properties?.Items;
            if (auth?.Principal == null || items == null || !items.ContainsKey(LoginProviderKey))
            {
                return null;
            }

            if (expectedXsrf != null)
            {
                if (!items.ContainsKey(XsrfKey))
                {
                    return null;
                }
                var userId = items[XsrfKey];
                if (userId != expectedXsrf)
                {
                    return null;
                }
            }

            var providerKey = GetProviderKey(auth.Principal);
            var provider = items[LoginProviderKey];
            if (providerKey == null || provider == null)
            {
                return null;
            }

            var authSchemas = await this.GetExternalAuthenticationSchemesAsync();
            var targetSchema = authSchemas.FirstOrDefault(p => p.Name == provider);
            var providerDisplayName = targetSchema?.DisplayName ?? provider;

            return new ExternalLoginInfo(auth.Principal, provider, providerKey, providerDisplayName)
            {
                AuthenticationTokens = auth.Properties.GetTokens()
            };
            //return base.GetExternalLoginInfoAsync(expectedXsrf);
        }

        #region Private methods

        private async Task CreateAuditReport(SignInResult result, User user)
        {
            var report = new UserAudit();
            var headers = Newtonsoft.Json.JsonConvert.SerializeObject(this.ContextAccessor.HttpContext.Request.Headers);
            var ip = this.ContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (result == null) {
                report = UserAudit.CreateAuditEvent(user.Id.ToString(), UserAuditEventType.LogOut, ip, headers: headers);
            } else {
                switch (result.ToString())
                {
                    case "Failed":
                        report = UserAudit.CreateAuditEvent(user.Id.ToString(), UserAuditEventType.FailedLogin, ip, "Failed due to invalid login", headers);
                        break;

                    case "Lockedout":
                        report = UserAudit.CreateAuditEvent(user.Id.ToString(), UserAuditEventType.FailedLogin, ip, "User account lockedout", headers);
                        break;

                    case "NotAllowed":
                        report = UserAudit.CreateAuditEvent(user.Id.ToString(), UserAuditEventType.FailedLogin, ip, "Login method either not allowed or account disabled", headers);
                        break;

                    case "RequiresTwoFactor":
                        report = UserAudit.CreateAuditEvent(user.Id.ToString(), UserAuditEventType.FailedLogin, ip, "Account login requires two factor authentication", headers);
                        break;

                    case "Succeeded":
                        report = UserAudit.CreateAuditEvent(user.Id.ToString(), UserAuditEventType.Login, ip, headers: headers);
                        break;
                }
            }

            await this.AuditRepository.StoreUserAudit(report);
        }

        private string GetProviderKey(ClaimsPrincipal claims)
        {
            this.Logger.LogDebug("Build custom [ProviderKey] from provider based claims, instead of using [NameIdentifier] or [sub] claim");
            this.Logger.LogTrace("Custom [ProviderKey] build is used for case where the NameIdentifier claim is transient. This claim is a key used in base of .NET authentication layer, that must not be transient - must be static.");

            var nameIdentifier = claims.FindFirstValue(ClaimTypes.NameIdentifier);
            this.Logger.LogDebug($"Provider [NameIdentifier] claim value, {nameIdentifier}");

            this.Logger.LogDebug("Resolve [Authnmethodsreferences] claim value");
            var authnMethodRef = claims.FindFirstValue("http://schemas.microsoft.com/claims/authnmethodsreferences");

            this.Logger.LogDebug("Resolve provider [bank] claim value");
            var bank = claims.Claims.First(x => x.Type == $"{authnMethodRef}_bank").Value;

            this.Logger.LogDebug("Resolve provider [SSN] claim value");
            var ssn = claims.Claims.First(x => x.Type.Split('_')[1] == "ssn").Value;


            var personIdentifier = $"{authnMethodRef}:{bank}:{ssn}";

            return personIdentifier;
        }

        #endregion
    }
}
