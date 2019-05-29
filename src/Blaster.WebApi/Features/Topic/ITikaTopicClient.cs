using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Topic
{
    public interface ITikaTopicClient
    {
        Task<TopicListResponse> GetAll();
        Task CreateTopic(CreateTopicRequest topic);
    }
}