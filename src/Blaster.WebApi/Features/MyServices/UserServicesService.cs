using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.System;
using DFDS.TeamService.WebApi.Features.UserServices.model;
using Microsoft.Extensions.Configuration;

namespace Blaster.WebApi.Features.MyServices
{
    public class UserServicesService : IUserServicesService
    {
        private const string BlasterUrlKey = "BLASTER_URL";

        private readonly HttpClient _client;
        private readonly string _blasterBaseUri;
        private readonly IJsonSerializer _serializer;

        public UserServicesService(IConfiguration configuration, HttpClient client, IJsonSerializer serializer)
        {
            _blasterBaseUri = GetStringFromConfiguration(configuration, BlasterUrlKey);

            _client = client;
            _serializer = serializer;
        }

        private string GetStringFromConfiguration(IConfiguration configuration, string configurationKey)
        {
            var uriString = configuration[configurationKey];

            if (string.IsNullOrWhiteSpace(uriString))
            {
                throw new MissingConfigurationException($"Error, missing configuration value for \"{configurationKey}\".");
            }

            return uriString;
        }

        public async Task<TeamsDTO> GetServices(string userId)
        {
            var teams = await GetServicesFromTeamsService(userId);
            teams = ReBaseServiceLocations(_blasterBaseUri, teams);

            return teams;
        }

        public static TeamsDTO ReBaseServiceLocations(string baseUri, TeamsDTO teams)
        {
            foreach (var service in teams.Items.SelectMany(t => t.Services))
            {
                service.Location = baseUri.AddPath(service.Location);
            }
            
            return teams;
        }

        public async Task<TeamsDTO> GetServicesFromTeamsService(string userId)
        {
            var response = await _client.GetAsync($"api/users/{userId}/services");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var teams = _serializer.Deserialize<TeamsDTO>(content);

            return teams;
        }
    }
}