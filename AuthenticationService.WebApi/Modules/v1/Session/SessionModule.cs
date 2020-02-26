using Carter;
using Carter.Request;
using Carter.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.Session
{
    public class SessionModule : VersionModule
    {
        public SessionModule() : base("/session")
        {
            this.RequiresAuthorization();

            Get("/", async (req, res) =>
            {
                var user = req.HttpContext.User;
                
                await res.Negotiate(user);
            });
        }
    }
}
