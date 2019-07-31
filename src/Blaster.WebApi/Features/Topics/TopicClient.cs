using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Topics.models;

namespace Blaster.WebApi.Features.Topics
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
        public async Task<TopicsResponse> GetAll()
        {
            var response = await _client.GetAsync("/api/v1/topics");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TopicsResponse>(content);
        }

        public async Task<Topic> GetById(string id)
        {
            var response = await _client.GetAsync($"/api/v1/topics/{id}");
            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"An error occured trying to reach {response.RequestMessage.RequestUri}, the http response code is {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<Topic>(content);
        }

        public async Task<TopicsResponse> GetByCapabilityId(string id)
        {
            var response = await _client.GetAsync($"/api/v1/topics/by-capability-id/{id}");
            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"An error occured trying to reach {response.RequestMessage.RequestUri}, the http response code is {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TopicsResponse>(content);
        }
    }
}