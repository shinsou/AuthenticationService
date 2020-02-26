using AuthenticationService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.Login
{
    internal class ViewModel
    {
        /// <summary>
        /// Redirecto immediately to this path, with all request/session data
        /// </summary>
        public string RedirectUri { get; set; }
        /// <summary>
        /// Return to this url when login process is complete
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableLocalLogin { get; set; }
        /// <summary>
        /// Set of external providers associated with connected client
        /// </summary>
        public ExternalProvider[] ExternalProviders { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Defineds default state for login view to remember credentials next time
        /// </summary>
        public bool RememberLogin { get; set; }

        [Newtonsoft.Json.JsonProperty("__RequestVerificationToken")]
        public string RequestVerificationToken { get; set; }

        public ViewModel() { }

        public ViewModel(LoginResponse input)
        {
            this.EnableLocalLogin = input.EnableLocalLogin;
            this.ExternalProviders = input.ExternalProviders;
            this.RedirectUri = input.RedirectUri;
            this.RememberLogin = input.RememberLogin;
            this.ReturnUrl = input.ReturnUrl;
            this.Username = input.Username;
        }
    }
}
