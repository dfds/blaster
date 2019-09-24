using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Channels
{
    public interface IHaraldClient
    {
        Task<ChannelsResponse> GetAllChannels();
        Task JoinChannel(string channelId, string channelName, string clientId);
        Task LeaveChannel(string channelId, string clientId);
        Task<ConnectionsResponse> GetAllConnections(string clientName, string clientType, string clientId, string channelName, string channelType, string channelId);
    }
}