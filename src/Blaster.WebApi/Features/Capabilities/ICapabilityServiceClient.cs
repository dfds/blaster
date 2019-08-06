using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace Blaster.WebApi.Features.Capabilities
{
    public interface ICapabilityServiceClient
    {
        Task<CapabilitiesResponse> GetAll();
        Task<TopicsResponse> GetAllTopics();
        Task<Topic> GetTopic(string id);
        Task<Capability> CreateCapability(string name, string description);
        Task CreateTopic(string title, string description, string capabilityId, bool isPrivate);
        Task CreateMessageContract(string type, string description, string content, string topicId);
        Task RemoveMessageContract(string topicId, string type);
        Task<MessageContractsResponse> GetMessageContractsByTopicId(string topicId);
        Task<Capability> GetById(string id);
        Task JoinCapability(string capabilityId, string memberEmail);
        Task LeaveCapability(string capabilityId, string memberEmail);
        Task AddContext(string capabilityId, string contextName);
        Task AddTopic(string capabilityId, string topicName);
    }
}