using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Teams.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Teams
{
    [Route("api/teams")]
    [ApiController]
    public class TeamApiController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamApiController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("", Name = "GetAllTeams")]
        public async Task<ActionResult<TeamListResponse>> GetAll()
        {
            var teams = await _teamService.GetAll();

            return teams ?? new TeamListResponse
            {
                Items = new TeamListItem[0]
            };
        }

        [HttpGet("{id}", Name = "GetTeamById")]
        public async Task<ActionResult<TeamListItem>> GetById(string id)
        {
            var team = await _teamService.GetById(id);

            if (team != null)
            {
                return new ActionResult<TeamListItem>(team);
            }

            return new ActionResult<TeamListItem>(NotFound());

        }

        [HttpPost("", Name = "CreateTeam")]
        public async Task<CreatedAtRouteResult<TeamListItem>> CreateTeam([FromBody] TeamInput input)
        {
            var team = await  _teamService.CreateTeam(input.Name, input.Department);

            return new CreatedAtRouteResult<TeamListItem>(
                routeName: "GetTeamById",
                routeValues: new {id = team.Id},
                value: team
            );
        }

        [HttpPost("{id}/members", Name = "JoinTeam")]
        public async Task<ActionResult<User>> JoinTeam([FromRoute] string id, [FromBody] JoinTeamInput input)
        {
            if (string.IsNullOrWhiteSpace(input.UserId))
            {
                return new ActionResult<User>(BadRequest());
            }

            try
            {
                var user = await _teamService.JoinTeam(
                    teamId: id,
                    userId: input.UserId
                );

                return new ActionResult<User>(user);
            }
            catch (AlreadyJoinedException)
            {
                return new ActionResult<User>(Conflict(new
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
        public string UserId { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

}