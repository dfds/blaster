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

        public async Task JoinChannel(string channelId, string channelName, string senderId)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { ChannelId = channelId, ChannelName = channelName, SenderId = senderId}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            var response = await _client.PostAsync("/api/v1/channel/join", content);
            response.EnsureSuccessStatusCode();
        }
        
        public async Task LeaveChannel(string channelId, string senderId)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { ChannelId = channelId, SenderId = senderId}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            var response = await _client.PostAsync("/api/v1/channel/leave", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<ConnectionsResponse> GetAllConnections(string senderName, string senderType, string senderId, string channelName, string channelType, string channelId)
        {
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["senderName"] = senderName;
            query["senderType"] = senderType;
            query["senderId"] = senderId;
            query["channelName"] = channelName;
            query["channelType"] = channelType;
            query["channelId"] = channelId;
            
            var response = await _client.GetAsync($"/api/v1/connections?{query}");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<ConnectionsResponse>(content);            
        }
    }
}