using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;

namespace Blaster.WebApi.Features.Capabilities
{
	public interface IKafkaServiceClient : IForwardingClient
	{
		Task<TopicsResponse> GetByCapabilityId(string capabilityId);
	}
}