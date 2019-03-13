using System.Threading.Tasks;
using Blaster.WebApi.Features.Topic;

namespace Blaster.Tests.TestDoubles
{
    public class TopicClientStub : ITopicClient
    {
        public Task<TopicListResponse> GetAll()
        {
            return Task.FromResult<TopicListResponse>(null);
        }

        public Task CreateTopic(CreateTopicRequest topic)
        {
            return Task.CompletedTask;
        }
    }
}