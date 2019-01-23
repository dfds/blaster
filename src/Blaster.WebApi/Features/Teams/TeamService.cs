using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.WebApi.Features.Teams
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public TeamService(HttpClient client, JsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<TeamsResponse> GetAll()
        {
            var response = await _client.GetAsync("/api/v1/teams");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<TeamsResponse>(content);
        }

        public async Task<Team> CreateTeam(string name)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Name = name }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync("/api/v1/teams", content);
            if (response.StatusCode == HttpStatusCode.BadRequest) {
                var errorObj = _serializer.Deserialize<ErrorObject>(await response.Content.ReadAsStringAsync());
                throw new TeamValidationException(errorObj.Message);
            }
            else if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Error! Team was not created in external service. Service returned ({response.StatusCode} - {response.ReasonPhrase})");
            }

            var receivedContent = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<Team>(receivedContent);
        }

        public async Task<Team> GetById(string id)
        {
            var response = await _client.GetAsync($"/api/v1/teams/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return _serializer.Deserialize<Team>(content);
        }

        public async Task JoinTeam(string teamId, string memberEmail)
        {
            var content = new StringContent(
                content: _serializer.Serialize(new { Email = memberEmail }),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );

            var response = await _client.PostAsync($"/api/v1/teams/{teamId}/members", content);
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