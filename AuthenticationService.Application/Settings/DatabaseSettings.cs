using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Settings
{
    public class DatabaseSettings
    {
        private IConfiguration Configuration { get; }
        public DatabaseSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public string ConnectionString => this.Configuration.GetValue<string>("Database:ConnectionString");
        public string DatabaseInstanceName => this.Configuration.GetValue<string>("Database:InstanceName");
        
    }
}
