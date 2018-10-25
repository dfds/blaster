using System.Net.Http;
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

        public UserServicesApiController(
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
        public TeamsDTO GetServices(string userId)
        {
            var awsConsoleLogin = new ServiceDTO
            {
                Name = "AWS Console",
                Location = "/aws"
            };

            var teamAwesome = new TeamDTO{
                Name = "Awsome",
                Department = "Swimming",
                Services = new []{awsConsoleLogin}
            };

               var teamSecond = new TeamDTO{
                Name = "Second",
                Department = "Swimming",
                Services = new ServiceDTO[0]
            };

            var teamsDto = new TeamsDTO(){
                Items = new []{
                    teamAwesome, 
                    teamSecond
                }
            };


            return teamsDto;
        }
    }
}