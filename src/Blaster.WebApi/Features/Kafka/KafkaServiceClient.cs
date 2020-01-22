using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;

namespace Blaster.WebApi.Features.Capabilities
{
	public class KafkaServiceClient : IKafkaServiceClient
	{
		private readonly HttpClient _client;
		private readonly JsonSerializer _serializer;

		public KafkaServiceClient(HttpClient client, JsonSerializer serializer)
		{
			_client = client;
			_serializer = serializer;
		}

		public async Task<TopicsResponse> GetByCapabilityId(string capabilityId)
		{
			var response = await _client.GetAsync($"/api/v1/capabilities/{capabilityId}/topics");
			HttpResponseHelper.EnsureSuccessStatusCode(response);
			var content = await response.Content.ReadAsStringAsync();

			return _serializer.Deserialize<TopicsResponse>(content);
		}

		public async Task<Topic> CreateTopic(string capabilityId, CreateTopicRequest createTopicRequest)
		{
			var reqContent = new StringContent(
				content: _serializer.Serialize(createTopicRequest),
				encoding: Encoding.UTF8,
				mediaType: "application/json"
			);

			var response = await _client.PostAsync($"/api/v1/capabilities/{capabilityId}/topics", reqContent);
			HttpResponseHelper.EnsureSuccessStatusCode(response);
			
			var content = await response.Content.ReadAsStringAsync();

			return _serializer.Deserialize<Topic>(content);
		}

		public async Task ForwardHeader(string headerName, string headerValue)
		{
			_client.DefaultRequestHeaders.Add(headerName, headerValue);
		}
	}
}
