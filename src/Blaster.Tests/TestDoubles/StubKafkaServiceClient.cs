using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;

namespace Blaster.Tests.TestDoubles
{
	public class StubKafkaServiceClient : IKafkaServiceClient
	{
		public Task ForwardHeader(string headerName, string headerValue)
		{
			return Task.CompletedTask;
		}

		public Task<TopicsResponse> GetByCapabilityId(string capabilityId)
		{
			return Task.FromResult(new TopicsResponse());
		}

		public Task<TopicsResponse> GetAll()
		{
			return Task.FromResult(new TopicsResponse());
		}
		
		public Task<IEnumerable<KafkaCluster>> GetAllClusters()
		{
			return Task.FromResult(new List<KafkaCluster>().AsEnumerable());
		}

		public Task<Topic> CreateTopic(string capabilityId, CreateTopicRequest createTopicRequest)
		{
			return Task.FromResult(new Topic());
		}
	}
}
