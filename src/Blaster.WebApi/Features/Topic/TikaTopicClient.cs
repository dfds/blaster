using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;

namespace Blaster.WebApi.Features.Topic
{
    public class TikaTopicClient : ITopicClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public TikaTopicClient(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<TopicListResponse> GetAll()
        {
            var response = await _client.GetAsync("/api/topics");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TopicListResponse>(content);
        }

        public async Task CreateTopic(CreateTopicRequest topic)
        {
            var content = new StringContent(
                content: _serializer.Serialize(topic),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync("/api/topics", content);

            response.EnsureSuccessStatusCode();
        }
    }
}