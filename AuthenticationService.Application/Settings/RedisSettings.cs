using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Settings
{
    public class RedisSettings
    {
        private IConfiguration Configuration { get; }

        public RedisSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #region Public properties

        public bool AllowAdmin => this.Configuration.GetValue<bool?>("Redis:AllowAdmin") ?? true;
        public bool AbortConnect => this.Configuration.GetValue<bool?>("Redis:AbortConnection") ?? false;
        public string AppName => this.Configuration.GetValue<string>("Redis:AppName");
        public string Username => this.Configuration.GetValue<string>("Redis:Username");
        public string Password => this.Configuration.GetValue<string>("Redis:Password");
        public string Host => this.Configuration.GetValue<string>("Redis:Host");
        public string DataProtectionKeyName => this.Configuration.GetValue<string>("Redis:DataProtectionKeyName");
        public int Port => this.Configuration.GetValue<int>("Redis:Port");

        #endregion

        #region Public methods

        public string GetConnectionString()
        {
            var url = $"{this.Host + ":" + this.Port}, abortConnect={this.AbortConnect}, allowAdmin={this.AllowAdmin} {SetPasswordToConnStringIfPresented(this.Password)}";

            if (!String.IsNullOrWhiteSpace(this.Password))
                url += $", password={this.Password}";

            return url;
        }

        #endregion

        #region Private methods

        private string SetPasswordToConnStringIfPresented(string password)
            => !String.IsNullOrWhiteSpace(password)
                ? $", password={password}"
                : "";

        #endregion
    }
}
