using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthenticationService.Application.Settings
{
    public class AuthenticationSettings
    {
        private IConfiguration Configuration { get; }
        public AuthenticationSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public List<AuthenticationProviderSettings> Providers
            => this.Configuration
                .GetSection("Authentication:Providers")
                ?.Get<List<AuthenticationProviderSettings>>();
    }

    public class AuthenticationProviderSettings
    {
        public string Authority { get; set; }
        public string CallbackPath { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string ResponseType { get; set; }
        public string Schema { get; set; }
        public string Secret { get; set; }
        public string SignedOutCallbackPath { get; set; }
        public string SignInScheme { get; set; }
        public string SignOutScheme { get; set; }
        public string[] Scopes { get; set; }
    }
}
