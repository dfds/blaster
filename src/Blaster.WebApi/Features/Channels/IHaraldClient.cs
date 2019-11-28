using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;

namespace Blaster.WebApi.Features.Channels
{
    public interface IHaraldClient : IForwardingClient
    {
        Task<ChannelsResponse> GetAllChannels();
        Task JoinChannel(string channelId, string channelName, string channelType, string clientId, string clientName, string clientType);
        Task LeaveChannel(string channelId, string channelType, string clientId, string clientType);
        Task<ConnectionsResponse> GetAllConnections(string clientName, string clientType, string clientId, string channelName, string channelType, string channelId);
        Task<ConnectionsResponse> GetConnectionsByCapabilityId(string id);
    }
}