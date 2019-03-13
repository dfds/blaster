using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Topic
{
    public interface ITopicClient
    {
        Task<TopicListResponse> GetAll();
        Task CreateTopic(CreateTopicRequest topic);
    }
}