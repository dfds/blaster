using System;
using System.Threading.Tasks;

namespace Blaster.WebApi
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        public Task<bool> IsValid(string apiKey)
        {
            var apiKeys = Environment.GetEnvironmentVariable("blaster_apikey") ?? "";
            var isValid = apiKeys.Contains(apiKey);

            return Task.FromResult(isValid);
        }
    }
}