using Carter.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.ModuleHelpers
{
    public class CustomJsonModelBinder : IModelBinder
    {
        public async Task<T> Bind<T>(HttpRequest request)
            => await Bind<T>(request, false);

        /// <summary>
        /// Custom json model binder for override default binder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public async Task<T> Bind<T>(HttpRequest request, bool caseSensitive = false)
        {
            var loggerFactory = (ILoggerFactory)request.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger<CustomJsonModelBinder>();

            // enable request body for multiple reads
            request.EnableBuffering();

            try
            {
                using var streamReader = new StreamReader(request.Body);
                var jsonResult = await streamReader.ReadToEndAsync();
                
                // reset stream position
                request.Body.Position = 0;

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);

                return result;
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, ex.Message);

                return typeof(T).IsValueType == false ? Activator.CreateInstance<T>() : default;
            }
        }
    }
}
