using Carter;
using Carter.Request;
using Carter.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules
{
    public abstract class ApiModule : CarterModule
    {
        public ApiModule(string route) : base($"/api{route}") { }
    }
}
