using System.Collections.Generic;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilities
{
	[Route("api/kafka/cluster")]
	[ForwardHeader]
	[ApiController]
	public class KafkaClusterApiController : ControllerBase
	{
		private readonly IKafkaServiceClient _KafkaServiceClient;

		public KafkaClusterApiController(IKafkaServiceClient kafkaServiceClient)
		{
			_KafkaServiceClient = kafkaServiceClient;
		}
		
		public void ForwardHeaders()
		{
			ForwardHeader.ForwardMsal(
				request: Request,
				client: _KafkaServiceClient);
		}

		[HttpGet("")]
		public async Task<ActionResult<IEnumerable<KafkaCluster>>> GetAll()
		{
			try
			{
				var clustersResponse = await _KafkaServiceClient.GetAllClusters();

				if (clustersResponse != null)
				{
					return new ActionResult<IEnumerable<KafkaCluster>>(clustersResponse);
				}

				return new ActionResult<IEnumerable<KafkaCluster>>(NotFound());
			}
			catch (UnauthorizedException)
			{
				return Unauthorized();
			}
		}
	}
}
