using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Channels
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

        public async Task JoinChannel(string channelId, string channelName, string clientId)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { ChannelId = channelId, ChannelName = channelName, ClientId = clientId}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            var response = await _client.PostAsync("/api/v1/channel/join", content);
            response.EnsureSuccessStatusCode();
        }
        
        public async Task LeaveChannel(string channelId, string clientId)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { ChannelId = channelId, ClientId = clientId}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            var response = await _client.PostAsync("/api/v1/channel/leave", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<ConnectionsResponse> GetAllConnections(string clientName, string clientType, string clientId, string channelName, string channelType, string channelId)
        {
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["clientName"] = clientName;
            query["clientType"] = clientType;
            query["clientId"] = clientId;
            query["channelName"] = channelName;
            query["channelType"] = channelType;
            query["channelId"] = channelId;
            
            var response = await _client.GetAsync($"/api/v1/connections?{query}");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<ConnectionsResponse>(content);            
        }
    }
}