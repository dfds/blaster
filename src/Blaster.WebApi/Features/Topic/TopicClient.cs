using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.WebApi.Features.Topic
{
    public class TopicClient : ITopicClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;
        private const string BASE_PATH = "/api/v1/topics";
        public TopicClient(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task CreateTopic(
            string name, 
            string description, 
            string visibility
        )
        {
            var content = new StringContent(
                content: _serializer.Serialize(new
                {
                    Name = name,
                    Description = description,
                    Visibility = visibility
                }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            
            var response = await _client.PostAsync(BASE_PATH, content);
            if (response.StatusCode == HttpStatusCode.BadRequest) {
                var errorObj = _serializer.Deserialize<ErrorObject>(await response.Content.ReadAsStringAsync());
                throw new TopicValidationException(errorObj.Message);
            }
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Error! Topic was not created in external service. Service returned ({response.StatusCode} - {response.ReasonPhrase})");
            }
        }
        
        public async Task<Topic> GetByName(string name)
        {
               var response = await _client.GetAsync(BASE_PATH  + $"/{name}");
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

            await _client.PostAsync(BASE_PATH  + $"/{topicName}/messageexamples", content);
        }
    }
}