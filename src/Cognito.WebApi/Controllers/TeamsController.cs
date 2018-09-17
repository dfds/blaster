using System.Collections.Generic;
using System.Threading.Tasks;
using Cognito.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly CognitoClient _cognitoClient;

        public TeamsController(CognitoClient cognitoClient)
        {
            _cognitoClient = cognitoClient;
        }
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

        
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(409)]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeam createTeam)
        {
            var existingTeam = await _cognitoClient.GetGroupAsync(createTeam.Name);
            if (existingTeam != null)
            {
                return Conflict(new {teamName = $"a team with the name {createTeam.Name} already exists"});
            }

            await _cognitoClient.CreateGroupAsync(createTeam.Name);
            
            //             return CreatedAtRoute("Get", new {id = createTeam.Name}, createTeam);

            return CreatedAtAction("GetTeam", new {id = createTeam.Name}, createTeam);
        }
    }
}