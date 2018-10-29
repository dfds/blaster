using System;
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
        private const string BlasterUrlKey = "BLASTER_URL";
        private const string TeamServiceApiUrlKey = "BLASTER_TEAMSERVICE_API_URL";

        private readonly HttpClient _client;
        private readonly Uri _blasterBaseUri;
        private readonly Uri _teamsBaseUri;
        private readonly IJsonSerializer _serializer;


        public UserServicesApiController(
            IConfiguration configuration,
            HttpClient client,
            IJsonSerializer serializer
        )
        {
            if (string.IsNullOrWhiteSpace(configuration[BlasterUrlKey]))
            {
                throw new MissingConfigurationException(
                    $"Error, missing configuration value for \"{BlasterUrlKey}\".");
            }

            _blasterBaseUri = new Uri(configuration[BlasterUrlKey]);
        
            
            if (string.IsNullOrWhiteSpace(configuration[TeamServiceApiUrlKey]))
            {
                throw new MissingConfigurationException(
                    $"Error, missing configuration value for \"{TeamServiceApiUrlKey}\".");
            }

            _teamsBaseUri = new Uri(configuration[TeamServiceApiUrlKey]);

            _client = client;
            _serializer = serializer;
        }


        [HttpGet("api/users/{userId}/services")]
        public async Task<TeamsDTO> GetServices(string userId)
        {
            var url = new Uri(
                _teamsBaseUri,
                $"/api/users/{userId}/services"
            );

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var teams = _serializer.Deserialize<TeamsDTO>(content);

            foreach (var service in teams.Items.SelectMany(t => t.Services))
            {
                var serviceUrl = new Uri(_blasterBaseUri, service.Location);
                service.Location = serviceUrl.OriginalString;
            }


            return teams;
        }
    }
}