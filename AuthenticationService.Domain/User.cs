using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain
{
    public class User : IdentityUser<Guid>
    {
        public bool? IsEnabled { get; set; }
        public bool? RequireChangePassword { get; set; }
    }
}
