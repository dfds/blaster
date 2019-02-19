using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.WebApi.Features.Teams
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