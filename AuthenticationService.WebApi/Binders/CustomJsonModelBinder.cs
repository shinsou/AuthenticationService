﻿using Carter.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Binders
{
    public class CustomJsonModelBinder : IModelBinder
    {
        public async Task<T> Bind<T>(HttpRequest request)
            => await Bind<T>(request, true);

        public async Task<T> Bind<T>(HttpRequest request, bool caseSensitive = false)
        {
            var loggerFactory = (ILoggerFactory)request.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger<CustomJsonModelBinder>();
            try
            {
                //request.Body

                var result = await JsonSerializer.DeserializeAsync<T>(request.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = caseSensitive });

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
