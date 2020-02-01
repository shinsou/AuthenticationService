using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthenticationService.Infrastructure.Extensions
{
    public static class IdentityUserExtensions
    {
        public static bool IsLdapUser(this IList<System.Security.Claims.Claim> claims)
        {
            return claims.Any(c => c.Type.ToLower() == "usertype" && c.Value.ToLower() == "ldap");
        }
    }
}
