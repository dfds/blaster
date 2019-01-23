using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Teams
{
    [Route("api/teams")]
    [ApiController]
    public class TeamApiController : ControllerBase
    {
        private readonly ITeamServiceClient _teamServiceClient;

        public TeamApiController(ITeamServiceClient teamServiceClient)
        {
            _teamServiceClient = teamServiceClient;
        }

        [HttpGet("", Name = "GetAllTeams")]
        public async Task<ActionResult<TeamsResponse>> GetAll()
        {
            var teams = await _teamServiceClient.GetAll();

            return teams ?? new TeamsResponse
            {
                Items = new Team[0]
            };
        }

        [HttpGet("{id}", Name = "GetTeamById")]
        public async Task<ActionResult<Team>> GetById(string id)
        {
            var team = await _teamServiceClient.GetById(id);

            if (team != null)
            {
                return new ActionResult<Team>(team);
            }

            return new ActionResult<Team>(NotFound());

        }

        [HttpPost("", Name = "CreateTeam")]
        public async Task<CreatedAtRouteResult<Team>> CreateTeam([FromBody] TeamInput input)
        {
            var team = await  _teamServiceClient.CreateTeam(input.Name);

            return new CreatedAtRouteResult<Team>(
                routeName: "GetTeamById",
                routeValues: new {id = team.Id},
                value: team
            );
        }

        [HttpPost("{id}/members", Name = "JoinTeam")]
        public async Task<ActionResult<Member>> JoinTeam([FromRoute] string id, [FromBody] JoinTeamInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                return new ActionResult<Member>(BadRequest());
            }

            try
            {
                await _teamServiceClient.JoinTeam(
                    teamId: id,
                    memberEmail: input.Email
                );

                return new ActionResult<Member>(NoContent());
            }
            catch (AlreadyJoinedException)
            {
                return new ActionResult<Member>(Conflict(new
                {
                    Message = "User is already part of the team"
                }));
            }
        }
    }

    public class AlreadyJoinedException : Exception
    {

    }

    public class JoinTeamInput
    {
        public string Email { get; set; }
    }
}