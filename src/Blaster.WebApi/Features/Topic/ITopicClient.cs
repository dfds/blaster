using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Topic
{
    public interface ITopicClient
    {
        Task<Topic> GetByName(string topicName);
        Task CreateMessageExample(string topicName, string messageType, string messageText);
    }
}