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
    }
}