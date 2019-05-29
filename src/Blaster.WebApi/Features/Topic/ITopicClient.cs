using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Topic
{
    public interface ITopicClient
    {
        Task<Topic> GetById(string topicId);
    }
}