using System;
using System.Data;
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

        public async Task JoinChannel(string channelId, string channelName, string channelType, string clientId, string clientName, string clientType)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { ChannelId = channelId, ChannelName = channelName, ChannelType = channelType, ClientId = clientId, ClientName = clientName, ClientType = clientType}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            var response = await _client.PostAsync("/api/v1/connections", content);
            response.EnsureSuccessStatusCode();
        }
        
        public async Task LeaveChannel(string channelId, string channelType, string clientId, string clientType)
        {
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["clientType"] = clientType;
            query["clientId"] = clientId;
            query["channelType"] = channelType;
            query["channelId"] = channelId;
            
            var content = new StringContent(
                content: _serializer.Serialize(new { ChannelId = channelId, ClientId = clientId, ChannelType = channelType, ClientType = clientType}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            var response = await _client.DeleteAsync($"/api/v1/connections?{query}");
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

        public async Task<ConnectionsResponse> GetConnectionsByCapabilityId(string id)
        {
            return await GetAllConnections(null, null, id, null, null, null);
        }
    }
}