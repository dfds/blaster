using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubCapabilityServiceClient : ICapabilityServiceClient
    {
        private readonly Member _member;
        private readonly List<Capability> _capabilities;
        private readonly Topic[] _topics;
        private readonly MessageContract[] _messageContracts;

        public StubCapabilityServiceClient(Member member = null, params Capability[] capabilities)
        {
            if (capabilities.Any() == false)
            {
                _capabilities = new List<Capability>();
            }
            else
            {
                _capabilities = capabilities.ToList();
            }

            _member = member;
            _topics = null;
            _messageContracts = null;
        }

        public Task<CapabilitiesResponse> GetAll()
        {
            return Task.FromResult(new CapabilitiesResponse
            {
                Items = _capabilities.ToArray(),
            });
        }

        public Task<TopicsResponse> GetAllTopics()
        {
            return Task.FromResult(new TopicsResponse
            {
                Items = _topics,
            });
        }

        public Task<Topic> GetTopic(string id)
        {
            return Task.FromResult(new Topic());
        }

        public Task<Capability> CreateCapability(string name, string description)
        {
            return Task.FromResult(_capabilities.First());
        }

        public Task DeleteCapability(string capabilityId)
        {
            var capabilityToDelete = _capabilities.Single(c => c.Id == capabilityId);
            _capabilities.Remove(capabilityToDelete);
            
            return Task.CompletedTask;
        }

        public Task UpdateCapability(string capabilityId, string name, string description)
        {
            return Task.CompletedTask;
        }

        public Task CreateTopic(string title, string description, string capabilityId, bool isPrivate)
        {
            return Task.CompletedTask;
        }

        public Task UpdateTopic(string topicId, Topic input)
        {
            return Task.CompletedTask;
        }

        public Task CreateMessageContract(string type, string description, string content, string topicId)
        {
            return Task.CompletedTask;
        }

        public Task AddUpdateMessageContract(string type, string topicId, MessageContractInput input)
        {
            return Task.CompletedTask;
        }

        public Task RemoveMessageContract(string topicId, string type)
        {
            return Task.CompletedTask;
        }

        public Task<MessageContractsResponse> GetMessageContractsByTopicId(string topicId)
        {
            return Task.FromResult(new MessageContractsResponse
            {
                Items = _messageContracts,
            });
        }

        public Task<Capability> GetById(string id)
        {
            return Task.FromResult(_capabilities.FirstOrDefault());
        }

        public Task JoinCapability(string capabilityId, string memberEmail)
        {
            return Task.CompletedTask;
        }

        public Task LeaveCapability(string capabilityId, string memberEmail)
        {
            return Task.CompletedTask;
        }

        public Task AddContext(string capabilityId, string contextName)
        {
            return Task.CompletedTask;
        }

        public Task AddTopic(string capabilityId, string topicName)
        {
            return Task.CompletedTask;
        }
    }
}