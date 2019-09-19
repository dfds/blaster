using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.CommunicationChannels.Models;

namespace Blaster.WebApi.Features.CommunicationChannels
{
    public class HaraldServiceClient : IHaraldServiceClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public HaraldServiceClient(
            HttpClient client, 
            JsonSerializer serializer
        )
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<ConnectionsResponse> GetConnectionsByCapabilityIdAsync(string capabilityId)
        {
            var response = await _client.GetAsync($"/api/v1/connections?senderType=capability&senderId={capabilityId}");
            
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return _serializer.Deserialize<ConnectionsResponse>(content);
        }
    }
}