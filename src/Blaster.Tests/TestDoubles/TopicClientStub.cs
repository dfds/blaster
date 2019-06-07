using System.Threading.Tasks;
using Blaster.WebApi.Features.Topic;

namespace Blaster.Tests.TestDoubles
{
    public class TopicClientStub : ITopicClient
    {
        public Task<Topic> GetByName(string topicName)
        {
            return Task.FromResult<Topic>(null);
        }

        public Task CreateMessageExample(string topicName, string messageType, string messageText)
        {
            return Task.CompletedTask;
        }

        public Task CreateTopic(string name)
        {
            return Task.CompletedTask;
        }
    }
}