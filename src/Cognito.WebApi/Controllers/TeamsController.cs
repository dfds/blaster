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
        public async Task<ActionResult<Team>> GetTeam(
            string teamId,
            string departmentId
        )
        {
            var team = await _teamsService.GetTeam(teamId, departmentId);

            return team;
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(409)]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeam createTeam)
        {


            var result = await _teamsService.CreateTeam(createTeam);


            return result.Reduce<ActionResult>(
                team => CreatedAtAction(nameof(GetTeam), new {id = team.Id}, createTeam),
                failure =>
                {
                    // TODO return 
                    return Conflict(failure.Message);
                }
            );
        }
    }
}