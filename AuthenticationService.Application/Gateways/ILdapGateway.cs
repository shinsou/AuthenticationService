using AuthenticationService.Core;
using AuthenticationService.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Gateways
{
    public interface ILdapGateway<T> where T : IdentityUser<Guid>
    {
        Task<CustomLdapResponse> AuthenticateAsync(string username, string password);
    }
}
