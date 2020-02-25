using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public string RedirectUri { get; set; }
        public string ReturnUrl { get; set; }
        public bool EnableLocalLogin { get; set; }
        public ExternalProvider[] ExternalProviders { get; set; }
        public string Username { get; set; }
        public bool RememberLogin { get; set; }
    }
}
