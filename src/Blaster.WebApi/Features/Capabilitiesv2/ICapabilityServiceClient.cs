using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilitiesv2.Models;

namespace Blaster.WebApi.Features.Capabilitiesv2
{
    public interface ICapabilityServiceClient
    {
        Task<CapabilitiesResponse> GetAll();
        Task<Capability> CreateCapability(string name);
        Task<Capability> GetById(string id);
        Task JoinCapability(string capabilityId, string memberEmail);
        Task LeaveCapability(string capabilityId, string memberEmail);
    }
}