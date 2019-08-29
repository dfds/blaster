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
        Task DeleteCapability(string capabilityId);
        Task UpdateCapability(string capabilityId, string name, string description);
        Task SetCapabilityTopicCommonPrefix(string commonPrefix, string capabilityId);
        Task CreateTopic(string title, string description, string capabilityId, bool isPrivate, string businessArea, string type, string misc);
        Task UpdateTopic(string topicId, Topic input);
        Task CreateMessageContract(string type, string description, string content, string topicId);
        Task AddUpdateMessageContract(string type, string topicId, MessageContractInput input);
        Task RemoveMessageContract(string topicId, string type);
        Task<MessageContractsResponse> GetMessageContractsByTopicId(string topicId);
        Task<Capability> GetById(string id);
        Task JoinCapability(string capabilityId, string memberEmail);
        Task LeaveCapability(string capabilityId, string memberEmail);
        Task AddContext(string capabilityId, string contextName);
        Task AddTopic(string capabilityId, string topicName);
    }
}