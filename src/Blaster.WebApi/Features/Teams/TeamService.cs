using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.WebApi.Features.Teams
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _client;
        private readonly IJsonSerializer _serializer;

        public TeamService(HttpClient client, IJsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<TeamListResponse> GetAll()
        {
            var response = await _client.GetAsync("/api/teams");
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

            var response = await _client.PostAsync("/api/teams", content);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Error! Team was not created in external service. Service returned ({response.StatusCode} - {response.ReasonPhrase})");
            }

            var receivedContent = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<TeamListItem>(receivedContent);
        }

        public async Task<TeamListItem> GetById(string id)
        {
            var response = await _client.GetAsync($"/api/teams/{id}");
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

            var response = await _client.PostAsync($"/api/teams/{teamId}/members", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new AlreadyJoinedException();
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServerReturnedUnexpectedResponseException($"{response.StatusCode:D} - {response.ReasonPhrase}", responseBody);
            }

            var member = _serializer.Deserialize<Member>(responseBody);
            if (member == null)
            {
                throw new Exception("Error, unable to deserialize response body into team member instance. Reponse body was: " + responseBody);
            }

            return member;
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