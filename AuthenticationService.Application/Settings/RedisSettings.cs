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

        public string AppName => this.Configuration.GetValue<string>("Redis:AppName");
        public string ConnectionString => this.Configuration.GetValue<string>("Redis:ConnectionString");
    }
}
