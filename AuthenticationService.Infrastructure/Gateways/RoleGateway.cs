using AuthenticationService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Gateways
{
    public class RoleGateway : RoleManager<Role>
    {
        new private ILogger<RoleGateway> Logger { get; }
        public RoleGateway(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILoggerFactory loggerFactory)
            : base(store, roleValidators, keyNormalizer, errors, loggerFactory.CreateLogger<RoleManager<Role>>())
        {
            this.Logger = loggerFactory.CreateLogger<RoleGateway>();
        }

        //#TODO
    }
}
