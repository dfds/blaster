using System.Threading.Tasks;
using Blaster.WebApi.Features.CommunicationChannels.Models;

namespace Blaster.WebApi.Features.CommunicationChannels
{
    public interface IHaraldServiceClient
    {
        Task<ConnectionsResponse> GetConnectionsByCapabilityIdAsync(string capabilityId);
    }
}