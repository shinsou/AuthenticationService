using Carter;
using Carter.Request;
using Carter.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.Dashboard
{
    public class DashboardModule : VersionModule
    {
        public DashboardModule(ILogger<DashboardModule> logger) : base("/dashboard")
        {
            Get("/", async (req, res) =>
            {
                await res.Negotiate(null);
            });
        }
    }
}
