using System.Threading.Tasks;
using Blaster.WebApi.Features.Topics.models;

namespace Blaster.WebApi.Features.Topics
{
    public interface ITopicClient
    {
        Task<TopicsResponse> GetAll();
        Task<Topic> CreateTopic(string title, string description, string capabilityId, bool isPublic);
        Task<Topic> GetById(string id);
        Task<TopicsResponse> GetByCapabilityId(string id);
    }
}