using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;

namespace Blaster.WebApi.Features.Capabilities
{
    public class CapabilityServiceClient : ICapabilityServiceClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public CapabilityServiceClient(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<CapabilitiesResponse> GetAll()
        {
            var response = await _client.GetAsync("/api/v1/capabilities");
            HttpResponseHelper.EnsureSuccessStatusCode(response);
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<CapabilitiesResponse>(content);
        }

        public async Task<TopicsResponse> GetAllTopics()
        {
            var response = await _client.GetAsync("/api/v1/topics");
            HttpResponseHelper.EnsureSuccessStatusCode(response);
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TopicsResponse>(content);
        }

        public async Task<Topic> GetTopic(string id)
        {
            var response = await _client.GetAsync($"/api/v1/topics/{id}");
            HttpResponseHelper.EnsureSuccessStatusCode(response);
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<Topic>(content);
        }

        public async Task<Capability> CreateCapability(string name, string description)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Name = name, Description = description}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync("/api/v1/capabilities", content);
            await HttpResponseHelper.MapStatusCodeToException(response);

            var receivedContent = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<Capability>(receivedContent);
        }

        public async Task DeleteCapability(string capabilityId)
        {
            var response = await _client.DeleteAsync($"/api/v1/capabilities/{capabilityId}");
            HttpResponseHelper.EnsureSuccessStatusCode(response);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Error! Capability was not deleted. Service returned ({response.StatusCode} - {response.ReasonPhrase})");
            }
        }

        public async Task UpdateCapability(string capabilityId, string name, string description)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new {Name = name, Description = description}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PutAsync($"/api/v1/capabilities/{capabilityId}", content);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorObj = _serializer.Deserialize<ErrorObject>(await response.Content.ReadAsStringAsync());
                throw new CapabilityValidationException(errorObj.Message);
            }
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task SetCapabilityTopicCommonPrefix(string commonPrefix, string capabilityId)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { CommonPrefix = commonPrefix}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            
            var response = await _client.PostAsync($"/api/v1/capabilities/{capabilityId}/commonPrefix", content);
            if (response.StatusCode == HttpStatusCode.BadRequest) {
                var errorObj = _serializer.Deserialize<ErrorObject>(await response.Content.ReadAsStringAsync());
                throw new CapabilityTopicValidationException(errorObj.Message);
            }
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }
        
        public async Task CreateTopic(string name, string description, string capabilityId, bool isPrivate)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new
                {
                    Name = name, Description = description, IsPrivate = isPrivate, MessageContract = new MessageContract[0]
                }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync($"/api/v1/capabilities/{capabilityId}/topics", content);
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task UpdateTopic(string topicId, Topic input)
        {
            var reqContent = new StringContent(
                content: _serializer.Serialize(new
                {
                    Description = input.Description, Name = input.Name, IsPrivate = true
                }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PutAsync($"/api/v1/topics/{topicId}", reqContent);
            var content = await response.Content.ReadAsStringAsync();
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task CreateMessageContract(string type, string description, string content, string topicId)
        {
            var reqContent = new StringContent(
                content: _serializer.Serialize(new { Type = type, Description = description, Content = content}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
                );

            var response = await _client.PostAsync($"/api/v1/topics/{topicId}/messageContracts", reqContent);
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }
        
        public async Task AddUpdateMessageContract(string type, string topicId, MessageContractInput input)
        {
            var reqContent = new StringContent(
                content: _serializer.Serialize(new {Description = input.Description, Content = input.Content}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PutAsync($"/api/v1/topics/{topicId}/messageContracts/{type}", reqContent);
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task RemoveMessageContract(string topicId, string type)
        {
            var response = await _client.DeleteAsync($"/api/v1/topics/{topicId}/messageContracts/{type}");
            var content = await response.Content.ReadAsStringAsync();
            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task<MessageContractsResponse> GetMessageContractsByTopicId(string topicId)
        {
            var response = await _client.GetAsync($"/api/v1/topics/{topicId}/messageContracts");
            HttpResponseHelper.EnsureSuccessStatusCode(response);

            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<MessageContractsResponse>(content);
        }

        public async Task<Capability> GetById(string id)
        {
            var response = await _client.GetAsync($"/api/v1/capabilities/{id}");
            HttpResponseHelper.EnsureSuccessStatusCode(response);
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<Capability>(content);
        }

        public async Task JoinCapability(string capabilityId, string memberEmail)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Email = memberEmail }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync($"/api/v1/capabilities/{capabilityId}/members", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new AlreadyJoinedException();
            }

            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task LeaveCapability(string capabilityId, string memberEmail)
        {
            var response = await _client.DeleteAsync($"/api/v1/capabilities/{capabilityId}/members/{memberEmail}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new UnknownCapabilityException();
            }

            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task AddContext(string capabilityId, string contextName)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Name = contextName }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync($"/api/v1/capabilities/{capabilityId}/contexts", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ContextAlreadyAddedException();
            }

            HttpResponseHelper.EnsureSuccessStatusCode(response);
        }

        public async Task AddTopic(string capabilityId, string topicName)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Name = topicName }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            await _client.PostAsync($"/api/v1/capabilities/{capabilityId}/topics", content);
        }

        public async Task ForwardHeader(string headerName, string headerValue)
        {
	        _client.DefaultRequestHeaders.Add(headerName, headerValue);
        }
    }

    public class ServerReturnedUnexpectedResponseException : Exception
    {
        public ServerReturnedUnexpectedResponseException(string message, string responseBody)
            : base(message)
        {
            ResponseBody = responseBody;
        }

        public string ResponseBody { get; }
    }
}
