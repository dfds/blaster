using System.Collections.Generic;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;

namespace Blaster.WebApi.Features.Capabilities
{
	public interface IKafkaServiceClient : IForwardingClient
	{
		Task<TopicsResponse> GetByCapabilityId(string capabilityId);
		Task<TopicsResponse> GetAll();
		Task<IEnumerable<KafkaCluster>> GetAllClusters();

		Task<Topic> CreateTopic(string capabilityId, CreateTopicRequest createTopicRequest);
	}
}
