using Carter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1
{
    public abstract class VersionModule : ApiModule
    {
        public VersionModule(string route, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") : base($"/v1{route}") { }
    }
}
