using AuthenticationService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Factories
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        private ILogger<ApplicationUserClaimsPrincipalFactory> Logger { get; }

        public ApplicationUserClaimsPrincipalFactory(UserManager<User> userManager,
                                                     RoleManager<IdentityRole> roleManager,
                                                     IOptions<IdentityOptions> optionsAccessor,
                                                     ILogger<ApplicationUserClaimsPrincipalFactory> logger)
                                                        : base(userManager, roleManager, optionsAccessor)
        {
            this.Logger = logger;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            //await SetCustomServiceClaims(identity, user);

            return identity;
        }
    }
}
