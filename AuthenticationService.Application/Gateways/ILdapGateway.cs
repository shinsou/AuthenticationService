using AuthenticationService.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Gateways
{
    public interface ILdapGateway<T> where T : IdentityUser<Guid>
    {
    }
}
