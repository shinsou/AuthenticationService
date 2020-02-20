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

        public bool AllowAdmin => this.Configuration.GetValue<bool?>("Redis:AllowAdmin") ?? true;

        public bool AbortConnect => this.Configuration.GetValue<bool?>("Redis:AbortConnection") ?? false;

        public string AppName => this.Configuration.GetValue<string>("Redis:AppName");

        public string ConnectionString
        {
            get
            {
                var url = $"{this.Host + ":" + this.Port}, abortConnect={this.AbortConnect}, allowAdmin={this.AllowAdmin}";

                if (!String.IsNullOrWhiteSpace(this.Password))
                    url += $", password={this.Password}";

                return url;
            }
        }

        public string Password => this.Configuration.GetValue<string>("Redis:Password");
        public string Host => this.Configuration.GetValue<string>("Redis:Host");
        public int Port => this.Configuration.GetValue<int>("Redis:Port");
    }
}
