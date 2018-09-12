using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blaster.WebApi.Features.System
{
    [Route("system")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;

        public HealthController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }

        [Route("health")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var greeting = await _cognitoService.SayHello();
            return greeting;
        }
    }

    public interface ICognitoService
    {
        Task<string> SayHello();
    }

    public class CognitoService : ICognitoService
    {
        private const string CognitoApiUrlKey = "BLASTER_COGNITO_API_URL";

        private readonly HttpClient _client;
        private readonly string _cognitoApiUrl;

        public CognitoService(IConfiguration configuration, HttpClient client)
        {
            _cognitoApiUrl = configuration[CognitoApiUrlKey];

            if (string.IsNullOrWhiteSpace(_cognitoApiUrl))
            {
                throw new MissingConfigurationException($"Error, missing configuration value for \"{CognitoApiUrlKey}\".");
            }

            _client = client;
        }

        public async Task<string> SayHello()
        {
            var response = await _client.GetAsync($"{_cognitoApiUrl}/sayhello");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}