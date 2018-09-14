using System.Collections.Generic;
using Cognito.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Team>> GetTeams()
        {
            var teams = new List<Team>
            {
                new Team
                {
                    Name = "Awesome",
                    Members = new List<User>
                    {
                        new User {Email = "kilin@dfds.com"},
                        new User {Email = "notme@dfds.com"}
                    }
                },
                new Team
                {
                    Name = "Nobody is home"
                }
            };


            return teams;
        }

        [HttpGet("{id}")]
        public ActionResult<Team> GetTeam(string id)
        {
            var team = new Team
            {
                Name = "Awesome",
                Members = new List<User>
                {
                    new User {Email = "kilin@dfds.com"},
                    new User {Email = "notme@dfds.com"}
                }
            };


            return team;
        }
    }
}