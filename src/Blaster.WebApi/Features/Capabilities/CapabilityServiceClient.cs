using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;

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
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<CapabilitiesResponse>(content);
        }

        public async Task<Capability> CreateCapability(string name, string description)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Name = name, Description = description}),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync("/api/v1/capabilities", content);
            if (response.StatusCode == HttpStatusCode.BadRequest) {
                var errorObj = _serializer.Deserialize<ErrorObject>(await response.Content.ReadAsStringAsync());
                throw new CapabilityValidationException(errorObj.Message);
            }
            else if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Error! Capability was not created in external service. Service returned ({response.StatusCode} - {response.ReasonPhrase})");
            }

            var receivedContent = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<Capability>(receivedContent);
        }

        public async Task<Capability> GetById(string id)
        {
            var response = await _client.GetAsync($"/api/v1/capabilities/{id}");
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

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServerReturnedUnexpectedResponseException($"{response.StatusCode:D} - {response.ReasonPhrase}", responseBody);
            }
        }

        public async Task LeaveCapability(string capabilityId, string memberEmail)
        {
            var response = await _client.DeleteAsync($"/api/v1/capabilities/{capabilityId}/members/{memberEmail}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new UnknownCapabilityException();
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServerReturnedUnexpectedResponseException($"{response.StatusCode:D} - {response.ReasonPhrase}", await response.Content.ReadAsStringAsync());
            }
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
