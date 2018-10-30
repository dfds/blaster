using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.System;
using DFDS.TeamService.WebApi.Features.UserServices.model;
using Microsoft.Extensions.Configuration;

namespace Blaster.WebApi.Features.MyServices
{
    public class UserServicesService :IUserServicesService
    {
        private const string BlasterUrlKey = "BLASTER_URL";
        private const string TeamServiceApiUrlKey = "BLASTER_TEAMSERVICE_API_URL";

        private readonly HttpClient _client;
        private readonly Uri _blasterBaseUri;
        private readonly Uri _teamsBaseUri;
        private readonly IJsonSerializer _serializer;


        public UserServicesService(
            IConfiguration configuration,
            HttpClient client,
            IJsonSerializer serializer
        )
        {
            _blasterBaseUri = GetUriFromConfiguration(configuration, BlasterUrlKey);
            _teamsBaseUri = GetUriFromConfiguration(configuration, TeamServiceApiUrlKey);
       
            _client = client;
            _serializer = serializer;
        }

        
        private Uri GetUriFromConfiguration(IConfiguration configuration, string configurationKey)
        {
            var uriString = configuration[configurationKey]; 
            if (string.IsNullOrWhiteSpace(uriString))
            {
                throw new MissingConfigurationException(
                    $"Error, missing configuration value for \"{configurationKey}\".");
            }


            if (uriString.EndsWith('/') == false)
            {
                uriString += '/';
            }

            
            return new Uri(uriString);
        }

        
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
                var serviceUrl = new Uri(_blasterBaseUri, CreateRelativeUri(service.Location));
                service.Location = serviceUrl.AbsoluteUri;
            }


            return teams;
        }

        private string CreateRelativeUri(string uri)
        {
            return uri.TrimStart('/');
        }   
    }
}