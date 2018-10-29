using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.System.Models;
using Microsoft.Extensions.Configuration;
using NotImplementedException = System.NotImplementedException;

namespace Blaster.WebApi.Features.System
{
    public class CognitoService : ICognitoService
    {
        private const string TeamServiceApiUrlKey = "BLASTER_TEAMSERVICE_API_URL";

        private readonly HttpClient _client;
        private readonly IJsonSerializer _serializer;
        private readonly string _teamApiUrl;

        public CognitoService(IConfiguration configuration, HttpClient client, IJsonSerializer serializer)
        {
            _teamApiUrl = configuration[TeamServiceApiUrlKey];

            if (string.IsNullOrWhiteSpace(_teamApiUrl))
            {
                throw new MissingConfigurationException($"Error, missing configuration value for \"{TeamServiceApiUrlKey}\".");
            }

            _client = client;
            _serializer = serializer;
        }

        public async Task<string> SayHello()
        {
            var response = await _client.GetAsync($"{_teamApiUrl}/system/health");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<AwsConsoleLinkResponse> GetAwsConsoleLink(Guid teamId, string idToken)
        {
            var response = await _client.GetAsync($"{_teamApiUrl}/api/teams/{teamId}/aws/console-url?idToken={idToken}");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<AwsConsoleLinkResponse>(content);
        }
    }
}