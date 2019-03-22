using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.AWSPermissions.Models;
using Blaster.WebApi.Features.Capabilities;

namespace DefaultNamespace
{
    public class AWSJanitorClient : IAWSJanitorClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public AWSJanitorClient(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<PolicyDTO[]> GetPoliciesByCapabilityNameAsync(string capabilityName)
        {
            var response = await _client.GetAsync("/api/policies/" +capabilityName);
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<PolicyDTO[]>(content);
        }
    }
}