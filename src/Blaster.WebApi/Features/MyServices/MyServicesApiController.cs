using System.Net.Http;
using Blaster.WebApi.Features.MyServices.Model;
using Blaster.WebApi.Features.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blaster.WebApi.Features.MyServices
{
    [ApiController]
    public class MyServiceController : ControllerBase
    {
        private const string TeamServiceApiUrlKey = "BLASTER_TEAMSERVICE_API_URL";

        private readonly HttpClient _client
            ;
        private readonly string _teamsBaseUri;

        public MyServiceController(
            IConfiguration configuration, 
            HttpClient client
        ){
            _client = client;
            _teamsBaseUri =configuration[TeamServiceApiUrlKey];

                 if (string.IsNullOrWhiteSpace(_teamsBaseUri))
            {
                throw new MissingConfigurationException($"Error, missing configuration value for \"{TeamServiceApiUrlKey}\".");
            }
        }

        [HttpGet("api/users/{userId}/services")]
        public TeamsResponse GetServices(string userId)
        {
            var awsConsoleLogin = new Service
            {
                Name = "AWS Console",
                Location = "/aws"
            };

            var teamAwesome = new Team{
                Name = "Awsome",
                Department = "Swimming",
                Services = new []{awsConsoleLogin}
            };

               var teamSecond = new Team{
                Name = "Second",
                Department = "Swimming",
                Services = new Service[0]
            };

            var teamsResponse = new TeamsResponse{
                Items = new []{
                    teamAwesome, 
                    teamSecond
                }
            };


            return teamsResponse;
        }
    }
}