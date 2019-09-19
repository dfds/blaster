using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Capabilities
{
    public interface IHaraldClient
    {
        Task<ChannelsResponse> GetAllChannels();
    }
}