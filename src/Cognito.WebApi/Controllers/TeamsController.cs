using System.Collections.Generic;
using System.Threading.Tasks;
using Cognito.WebApi.Failures;
using Cognito.WebApi.Model;
using Cognito.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
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


            return new TeamList{Items = teams};
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(
            string id
        )
        {
            var team = await _teamsService.GetTeam(id);

            return team;
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeam createTeam)
        {
            var result = await _teamsService.CreateTeam(createTeam);


            return result.Reduce<ActionResult>(
                team => CreatedAtAction(nameof(GetTeam), new {id = team.Id}, team),
                failure =>
                {
                    if (failure.GetType() == typeof(Conflict))
                    {
                        return Conflict(failure.Message);
                    }

                    if (failure.GetType() == typeof(ValidationFailed))
                    {
                        return BadRequest(failure.Message);
                    }

                    return StatusCode(500, failure.Message);
                }
            );
        }

        [HttpPost("{id}/members")]
        public async Task<ActionResult> JoinTeam(string id, [FromBody] JoinTeam joinTeam)
        {
          var result =  await _teamsService.JoinTeam(
              id, 
              joinTeam.UserId
          );


            return result.Reduce<ActionResult>(
                s => Ok(),
                netFound => NotFound(netFound.Message)
            );
        }
    }
}