using Blaster.WebApi.Features.MyServices.Model;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.MyServices
{
    [Route("api/myservices")]
    [ApiController]

    public class MyServiceSController : ControllerBase
    {
        [HttpGet("")]
        public TeamsResponse GetAll()
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