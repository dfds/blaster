using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.Tests.TestDoubles
{
    public class ErroneousCapabilityServiceClient : ICapabilityServiceClient
    {
        private readonly Exception _error;

        public ErroneousCapabilityServiceClient(Exception error)
        {
            _error = error;
        }

        public Task<CapabilitiesResponse> GetAll()
        {
            throw _error;
        }

        public Task<TopicsResponse> GetAllTopics()
        {
            throw _error;
        }

        public Task<Topic> GetTopic(string id)
        {
            throw _error;
        }

        public Task<Capability> CreateCapability(string name, string description)
        {
            throw _error;
        }

        public Task CreateTopic(string title, string description, string capabilityId, bool isPrivate)
        {
            throw _error;
        }

        public Task UpdateTopic(string topicId, Topic input)
        {
            throw _error;
        }

        public Task CreateMessageContract(string type, string description, string content, string topicId)
        {
            throw _error;
        }

        public Task AddUpdateMessageContract(string type, string topicId, MessageContractInput input)
        {
            throw _error;
        }

        public Task RemoveMessageContract(string topicId, string type)
        {
            throw _error;
        }

        public Task<MessageContractsResponse> GetMessageContractsByTopicId(string topicId)
        {
            throw _error;
        }

        public Task<Capability> GetById(string id)
        {
            throw _error;
        }

        public Task JoinCapability(string capabilityId, string memberEmail)
        {
            throw _error;
        }

        public Task LeaveCapability(string capabilityId, string memberEmail)
        {
            throw _error;
        }

        public Task AddContext(string capabilityId, string contextName)
        {
            throw _error;
        }

        public Task AddTopic(string capabilityId, string topicName)
        {
            throw _error;
        }
    }
}