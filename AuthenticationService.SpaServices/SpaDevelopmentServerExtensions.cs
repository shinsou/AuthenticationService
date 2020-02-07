using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace AuthenticationService.SpaServices
{
    public static class SpaDevelopmentServerExtensions
    {
        public static void UseSpaDevelopmentServer(this ISpaBuilder builder, string npmScript, string pkcMgrCommand = "npm", string npmArguments = "", string schema = "http", int port = 3000)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(builder.Options.SourcePath))
            {
                throw new InvalidOperationException($"To use {nameof(UseSpaDevelopmentServer)}, you must supply a non-empty value for the {nameof(SpaOptions.SourcePath)} property of {nameof(SpaOptions)} when calling {nameof(SpaApplicationBuilderExtensions.UseSpa)}.");
            }

            DevelopmentServerMiddleware.Attach(builder, npmScript, npmArguments, pkcMgrCommand, schema, port);
        }
    }
}
