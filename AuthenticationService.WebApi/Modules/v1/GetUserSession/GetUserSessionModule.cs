using Carter;
using Carter.Request;
using Carter.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.Session
{
    public class GetUserSessionModule : VersionModule
    {
        public GetUserSessionModule() : base("/session")
        {
            this.RequiresAuthorization();

            Get("/", async (req, res) =>
            {
                var userClaims = req.HttpContext.User.Claims;
                await res.Negotiate(userClaims.Select(item => new { item.Type, item.Value }));
            });
        }
    }
}
