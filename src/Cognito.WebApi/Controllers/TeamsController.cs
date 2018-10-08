using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cognito.WebApi.Failures;
using Cognito.WebApi.Model;
using Cognito.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamsService _teamsService;

        public TeamsController(
            TeamsService teamsService
        )
        {
            _teamsService = teamsService;
        }


        [HttpGet]
        public async Task<ActionResult<TeamList>> GetAllTeams()
        {
            var teams = await _teamsService.GetAllTeams();


            return new TeamList {Items = teams};
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(
            string id
        )
        {
            var team = await _teamsService.GetTeam(id);

            return team;
        }

        public class NegativeHttpResponse
        {
            public string Message { get; set; }
        }
        
        
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Team))]
        [ProducesResponseType(400, Type = typeof(NegativeHttpResponse))]
        [ProducesResponseType(409, Type = typeof(NegativeHttpResponse))]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeam createTeam)
        {
            var result = await _teamsService.CreateTeam(createTeam);

            return result.Reduce<ActionResult>(
                team => CreatedAtAction(
                    nameof(GetTeam),
                    new {team.Id},
                    team
                ),
                failure =>
                {
                    if (failure.GetType() == typeof(Conflict))
                    {
                        return Conflict(new NegativeHttpResponse {Message = failure.Message});
                    }

                    if (failure.GetType() == typeof(ValidationFailed))
                    {
                        return BadRequest(new NegativeHttpResponse {Message = failure.Message});
                    }

                    return StatusCode(500, failure.Message);
                }
            );
        }

        [HttpPost("{id}/members")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> JoinTeam(
            string id,
            [FromBody] JoinTeam joinTeam
        )
        {
            var joinTeamResult = await _teamsService.JoinTeam(
                id,
                joinTeam.UserId
            );


            return joinTeamResult.Reduce<ActionResult>(
                user => Ok(user),
                notFound => NotFound(notFound.Message)
            );
        }
    }
}