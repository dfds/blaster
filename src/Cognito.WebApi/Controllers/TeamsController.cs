using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognito.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly UserPoolClient _userPoolClient;

        public TeamsController(UserPoolClient userPoolClient)
        {
            _userPoolClient = userPoolClient;
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
        public async Task<ActionResult<Team>> GetTeam(string id)
        {
            var group = await _userPoolClient.GetGroupAsync(id);
            var usersInGroup = await _userPoolClient.ListUsersInGroupAsync(id);
            var team = new Team
            {
                Name = group.GroupName,
                Members = usersInGroup
                    .Select(u => 
                        new User {Id = u.Username}
                    ).ToList()
            };


            return team;
        }

        
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(409)]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeam createTeam)
        {
            var existingTeam = await _userPoolClient.GetGroupAsync(createTeam.Name);
            if (existingTeam != null)
            {
                return Conflict(new {teamName = $"a team with the name {createTeam.Name} already exists"});
            }

            await _userPoolClient.CreateGroupAsync(createTeam.Name);
            
            return CreatedAtAction(nameof(GetTeam), new {id = createTeam.Name}, createTeam);
        }
    }
}