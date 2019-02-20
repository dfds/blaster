using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Capabilities
{
    public interface ICapabilityServiceClient
    {
        Task<CapabilitiesResponse> GetAll();
        Task<Capability> CreateCapability(string name);
        Task<Capability> GetById(string id);
        Task JoinCapability(string teamId, string memberEmail);
        Task LeaveCapability(string teamId, string memberEmail);
    }
}