using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;

namespace Blaster.WebApi.Features.Topic
{
    public class TopicClient : ITopicClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public TopicClient(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }


        public async Task<Topic> GetByName(string name)
        {
               var response = await _client.GetAsync($"/api/v1/topics/{name}");
            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"An error occured trying to reach {response.RequestMessage.RequestUri}, the http response code is {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<Topic>(content);
 
        }

        public async Task CreateMessageExample(string topicName, string messageType, string messageText)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { MessageType = messageType, Text = messageText}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            await _client.PostAsync($"/api/v1/topics/{topicName}/messageexamples", content);
        }
    }
}