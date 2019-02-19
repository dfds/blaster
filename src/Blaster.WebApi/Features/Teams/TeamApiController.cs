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
        private readonly ICapabilityServiceClient _capabilityServiceClient;

        public TeamApiController(ICapabilityServiceClient capabilityServiceClient)
        {
            _capabilityServiceClient = capabilityServiceClient;
        }

        [HttpGet("", Name = "GetAllTeams")]
        public async Task<ActionResult<CapabilitiesResponse>> GetAll()
        {
            var teams = await _capabilityServiceClient.GetAll();

            return teams ?? new CapabilitiesResponse
            {
                Items = new Capability[0]
            };
        }

        [HttpGet("{id}", Name = "GetTeamById")]
        public async Task<ActionResult<Capability>> GetById(string id)
        {
            var team = await _capabilityServiceClient.GetById(id);

            if (team != null)
            {
                return new ActionResult<Capability>(team);
            }

            return new ActionResult<Capability>(NotFound());

        }

        [HttpPost("", Name = "CreateTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] TeamInput input)
        {
            try
            {
                var team = await _capabilityServiceClient.CreateCapability(input.Name);

                var a = new CreatedAtRouteResult<Capability>(
                    routeName: "GetTeamById",
                    routeValues: new { id = team.Id },
                    value: team
                );
                return a.Convert();
            } catch (TeamValidationException tve) {
                return BadRequest(new {
                    Message = tve.Message
                });
            }
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
                await _capabilityServiceClient.JoinCapability(
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

        [HttpDelete("{id}/members/{memberEmail}", Name = "LeaveTeam")]
        public async Task<IActionResult> LeaveTeam([FromRoute] string id, [FromRoute] string memberEmail)
        {
            try
            {
                await _capabilityServiceClient.LeaveCapability(id, memberEmail);
                return NoContent();                
            }
            catch (UnknownTeamException)
            {
                return NotFound();
            }
        }
    }

    public class AlreadyJoinedException : Exception
    {
    }

    public class TeamValidationException : Exception
    {
        public TeamValidationException(string message) : base(message)
        {

        }
    }

    public class UnknownTeamException : Exception
    {

    }

    public class JoinTeamInput
    {
        public string Email { get; set; }
    }
}