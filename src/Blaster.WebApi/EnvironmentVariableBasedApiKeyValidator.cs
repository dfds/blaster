using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blaster.WebApi
{
    public class EnvironmentVariableBasedApiKeyValidator : IApiKeyValidator
    {
        public Task<bool> IsValid(string apiKey)
        {
            var apiKeys = Environment.GetEnvironmentVariable("blaster_apikey") ?? "";
            var isValid = apiKeys
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Any(x => x == apiKey);

            return Task.FromResult(isValid);
        }
    }
}