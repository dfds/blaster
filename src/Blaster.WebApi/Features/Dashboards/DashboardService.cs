using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards.Models;

namespace Blaster.WebApi.Features.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _client;
        private readonly IJsonSerializer _serializer;
        private readonly string _baseUrl;

        public DashboardService(HttpClient client, ExternalDashboardServiceSettings settings, IJsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
            _baseUrl = $"{settings.ServiceEndpoint}/api/dashboards";
        }

        public async Task<DashboardListResponse> GetAll()
        {
            var result = await _client.GetStringAsync(_baseUrl);
            var response = _serializer.Deserialize<DashboardListResponse>(result);

            return response;
        }

        public async Task<DashboardDetailItem> Get(string id)
        {
            var url = $"{_baseUrl}/{id}";

            var result = await _client.GetStringAsync(url);
            var response = _serializer.Deserialize<DashboardDetailItem>(result);

            return response;
        }

        public async Task<DashboardDetailItem> Create(DashboardInput input)
        {
            var content = new StringContent(
                content: _serializer.Serialize(input),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync(_baseUrl, content);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception("Error! Dashboard was not created in external service.");
            }

            var receivedContent = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<DashboardDetailItem>(receivedContent);
        }

        public async Task<DashboardDetailItem> Update(DashboardInputWithId input)
        {
            var content = new StringContent(
                content: _serializer.Serialize(input),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PutAsync($"{_baseUrl}/{input.Id}", content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                    var receivedContent = await response.Content.ReadAsStringAsync();
                    return _serializer.Deserialize<DashboardDetailItem>(receivedContent);
                default:
                    throw new Exception($"Error! Falied to update dashboard with id {input.Id} in external service.");
            }
        }

        public async Task Delete(string id)
        {
            var response = await _client.DeleteAsync($"{_baseUrl}/{id}");

            if (response.StatusCode != HttpStatusCode.OK || response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception($"Error! Falied to delete dashboard with id {id} in external service.");
            }
        }
    }
}