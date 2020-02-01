using AuthenticationService.Application.Gateways;
using AuthenticationService.Domain;
using AuthenticationService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Gateways
{
    public class UserGateway : UserManager<User>
    {
        new private ILogger<UserGateway> Logger { get; }
        private ILdapGateway<User> LdapUserGateway { get; }

        public UserGateway(IUserStore<User> userStore,
                           ILdapGateway<User> ldapUserGateway,
                           IOptions<IdentityOptions> optionsAccessor,
                           IPasswordHasher<User> passwordHasher,
                           IEnumerable<IUserValidator<User>> userValidators,
                           IEnumerable<IPasswordValidator<User>> passwordValidators,
                           ILookupNormalizer keyNormalizer,
                           IdentityErrorDescriber errors,
                           IServiceProvider services,
                           ILoggerFactory loggerFactory)
            : base(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, loggerFactory.CreateLogger<UserManager<User>>())
        {
            this.Logger = loggerFactory.CreateLogger<UserGateway>();
            this.LdapUserGateway = ldapUserGateway;
        }
        //#TODO
        //protected override async Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
        //{
        //    var claims = await this.GetClaimsAsync(user);
        //    if (claims.IsLdapUser())
        //    {
        //        return await this.VerifyLdapPassword(user, password);
        //    }

        //    var hash = await store.GetPasswordHashAsync(user, CancellationToken);
        //    return hash == null
        //        ? PasswordVerificationResult.Failed
        //        : PasswordHasher.VerifyHashedPassword(user, hash, password);
        //}

        //public async Task<PasswordVerificationResult> VerifyLdapPassword(string username, string password)
        //    => await this.VerifyLdapPassword(await this.FindByNameAsync(username), password);

        //private async Task<PasswordVerificationResult> VerifyLdapPassword(User user, string password)
        //{
        //    if (user == null)
        //    {
        //        Logger.LogError("User reference null!");
        //        return PasswordVerificationResult.Failed;
        //    }

        //    var result = this.LdapUserStore.ValidateCredentials(user.UserName, password);

        //    if (result == null)
        //        return PasswordVerificationResult.Success;

        //    Logger.LogError(result.Exception, result.Message, user.UserName);
        //    return PasswordVerificationResult.Failed;
        //}

        //public override async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login)
        //{
        //    var result = await base.AddLoginAsync(user, login);

        //    return result;
        //}
    }
}
