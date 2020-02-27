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

        public bool Ssl => this.Configuration.GetValue<bool?>("redis:Ssl") ?? true;
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
            var stringBuilder = new StringBuilder($"{this.Host + ":" + this.Port}");
            stringBuilder.Append($",abortConnect={this.AbortConnect}");
            stringBuilder.Append($",allowAdmin={this.AllowAdmin}");
            stringBuilder.Append($",ssl={this.Ssl}");
            if (!String.IsNullOrWhiteSpace(this.Password)) {
                stringBuilder.Append($",password={this.Password}");
            }

            var url = stringBuilder.ToString();

            return url;
        }

        #endregion

        #region Private methods

        private string SetPasswordToConnStringIfPresented(string password)
            => !String.IsNullOrWhiteSpace(password)
                ? $", password={password}"
                : "";

        private string SetSslToConnectionStringIfPresented(bool? ssl)
            => ssl != null
                ? $", ssl={ssl}"
                : "";

        #endregion
    }
}
