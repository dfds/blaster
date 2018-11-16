using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.System.Models;
using Blaster.WebApi.Security;

namespace Blaster.WebApi.Features.System
{
    public class CognitoService : ICognitoService
    {
        private readonly HttpClient _client;
        private readonly IJsonSerializer _serializer;
        private readonly IdTokenAccessor _idTokenAccessor;

        public CognitoService(HttpClient client, IJsonSerializer serializer, IdTokenAccessor idTokenAccessor)
        {
            _client = client;
            _serializer = serializer;
            _idTokenAccessor = idTokenAccessor;
        }


        public async Task<string> SayHello()
        {
            var response = await _client.GetAsync("/system/health");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<AwsConsoleLinkResponse> GetAwsConsoleLink(Guid teamId)
        {
            var response = await _client.GetAsync($"/api/teams/{teamId}/aws/console-url?idToken={_idTokenAccessor.IdToken}");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<AwsConsoleLinkResponse>(content);
        }
    }
}