using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Channels
{
    public interface IHaraldClient
    {
        Task<ChannelsResponse> GetAllChannels();
        Task JoinChannel(string channelId, string channelName, string senderId);
        Task LeaveChannel(string channelId, string senderId);
        Task<ConnectionsResponse> GetAllConnections(string senderName, string senderType, string senderId, string channelName, string channelType, string channelId);
    }
}