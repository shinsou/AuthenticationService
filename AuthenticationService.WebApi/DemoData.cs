using AuthenticationService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi
{
    public static class DemoData
    {
        public static User DemoUser => new User
        {
            UserName = "demo",
            Email = "demo@demo.authenticationservice.io",
            EmailConfirmed = true,
            IsEnabled = true
        };

        public class UserInfo
        {
            public static string Password => "Q1w2e3r4!";
            public static string Name => $"{GivenName} {FamilyName}";
            public static string GivenName => "Demo";
            public static string FamilyName => "User";
            public static string Email => "demo.user@authenticationdserver.io";
            public static string EmailVerified => "true";
        }
    }
}
