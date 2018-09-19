using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognito.WebApi.Model;
using Cognito.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly UserPoolClient _userPoolClient;
        private readonly TeamsService _teamsService;

        public TeamsController(
            UserPoolClient userPoolClient,
            TeamsService teamsService
        )
        {
            _userPoolClient = userPoolClient;
            _teamsService = teamsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetAllTeams()
        {
            var teams = await _teamsService.GetAllTeams();


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
            var validationErrors = new List<string>();
            if (validationErrors.Any())
            {
                return UnprocessableEntity("");
            }

            
            
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