using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.System;
using DFDS.TeamService.WebApi.Features.UserServices.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blaster.WebApi.Features.MyServices
{
    [ApiController]
    public class UserServicesApiController : ControllerBase
    {
        private const string TeamServiceApiUrlKey = "BLASTER_TEAMSERVICE_API_URL";
        private readonly HttpClient _client
            ;
        private readonly string _teamsBaseUri;
        private readonly IJsonSerializer _serializer;


        public UserServicesApiController(
            IConfiguration configuration, 
            HttpClient client, 
            IJsonSerializer serializer
        ){
            
            _client = client;
            _serializer = serializer;
            _teamsBaseUri =configuration[TeamServiceApiUrlKey];

                 if (string.IsNullOrWhiteSpace(_teamsBaseUri))
            {
                throw new MissingConfigurationException($"Error, missing configuration value for \"{TeamServiceApiUrlKey}\".");
            }
        }

        
        [HttpGet("api/users/{userId}/services")]
        public async Task<TeamsDTO> GetServices(string userId)
        {
            var url = $"{_teamsBaseUri}/api/users/{userId}/services";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var teams =  _serializer.Deserialize<TeamsDTO>(content);

            
            foreach (var team in teams.Items)
            {
                foreach (var service in team.Services)
                {
                    service.Location = "https://localhost:5001" + service.Location;
                }
            }
            

            return teams;
        }
    }
}