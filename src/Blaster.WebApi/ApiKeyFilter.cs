using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blaster.WebApi
{
    public class ApiKeyFilter : IAuthorizationFilter
    {
        private readonly IApiKeyValidator _apiKeyValidator;

        public ApiKeyFilter(IApiKeyValidator apiKeyValidator)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (context.HttpContext.Request.Headers.TryGetValue("x-dfds-apikey", out var value))
            {
                var isValid = await _apiKeyValidator.IsValid(value.ToString());

                if (isValid)
                {
                    return;
                }
            }

            context.Result = new StatusCodeResult(403);
        }
    }
}