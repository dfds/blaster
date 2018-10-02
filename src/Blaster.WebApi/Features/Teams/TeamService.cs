using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams.Models;
using Microsoft.Extensions.Configuration;

namespace Blaster.WebApi.Features.Teams
{
    public class TeamService : ITeamService
    {
        private const string CognitoApiUrlKey = "BLASTER_COGNITO_API_URL";

        private readonly HttpClient _client;
        private readonly IJsonSerializer _serializer;
        private readonly string _baseUrl;

        public TeamService(IConfiguration configuration, HttpClient client, IJsonSerializer serializer)
        {
            _baseUrl = configuration[CognitoApiUrlKey];

            if (string.IsNullOrWhiteSpace(_baseUrl))
            {
                throw new MissingConfigurationException($"Error, missing configuration value for \"{CognitoApiUrlKey}\".");
            }

            _client = client;
            _serializer = serializer;
        }

        public async Task<TeamListResponse> GetAll()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/teams");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TeamListResponse>(content);
        }

        public async Task<TeamListItem> CreateTeam(string name, string department)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Name = name, Department = department }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync($"{_baseUrl}/api/teams", content);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Error! Team was not created in external service. Service returned ({response.StatusCode} - {response.ReasonPhrase})");
            }

            var receivedContent = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<TeamListItem>(receivedContent);
        }

        public async Task<TeamListItem> GetById(string id)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/teams/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TeamListItem>(content);
        }

        public async Task<Member> JoinTeam(string teamId, string userId)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { UserId = userId }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync($"{_baseUrl}/api/teams/{teamId}/members", content);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new AlreadyJoinedException();
            }

            var recievedContent = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<Member>(recievedContent);
        }
    }
}