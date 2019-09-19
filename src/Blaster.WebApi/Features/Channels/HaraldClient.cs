using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Capabilities
{
    public class HaraldClient : IHaraldClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public HaraldClient(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<ChannelsResponse> GetAllChannels()
        {
            var response = await _client.GetAsync("/api/v1/channels");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<ChannelsResponse>(content);
        }
    }
}