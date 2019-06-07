using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace Blaster.WebApi.Features.Capabilities
{
    public interface ICapabilityServiceClient
    {
        Task<CapabilitiesResponse> GetAll();
        Task<Capability> CreateCapability(string name, string description);
        Task<Capability> GetById(string id);
        Task JoinCapability(string capabilityId, string memberEmail);
        Task LeaveCapability(string capabilityId, string memberEmail);
        Task AddContext(string capabilityId, string contextName);
        Task AddTopic(string capabilityId, string topicName);
    }
}