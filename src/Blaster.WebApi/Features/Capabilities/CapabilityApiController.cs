using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilities
{
    [Route("api/capabilities")]
    [ApiController]
    public class CapabilityApiController : ControllerBase
    {
        private readonly ICapabilityServiceClient _capabilityServiceClient;

        public CapabilityApiController(ICapabilityServiceClient capabilityServiceClient)
        {
            _capabilityServiceClient = capabilityServiceClient;
        }

        [HttpGet("", Name = "GetAllCapabilities")]
        public async Task<ActionResult<CapabilitiesResponse>> GetAll()
        {
            var capabilities = await _capabilityServiceClient.GetAll();

            return capabilities ?? new CapabilitiesResponse
            {
                Items = new Capability[0]
            };
        }

        [HttpGet("{id}", Name = "GetCapabilityById")]
        public async Task<ActionResult<Capability>> GetById(string id)
        {
            var capability = await _capabilityServiceClient.GetById(id);

            if (capability != null)
            {
                return new ActionResult<Capability>(capability);
            }

            return new ActionResult<Capability>(NotFound());

        }

        [HttpPost("", Name = "CreateCapability")]
        public async Task<IActionResult> CreateCapability([FromBody] CapabilityInput input)
        {
            try
            {
                var capability = await _capabilityServiceClient.CreateCapability(input.Name, input.Description);

                var a = new CreatedAtRouteResult<Capability>(
                    routeName: "GetCapabilityById",
                    routeValues: new { id = capability.Id },
                    value: capability
                );
                return a.Convert();
            } catch (CapabilityValidationException tve) {
                return BadRequest(new {
                    Message = tve.Message
                });
            }
        }
        
        [HttpPost("{id}/topics", Name = "CreateTopic")]
        public async Task<ActionResult<Topic>> CreateTopic(string id, [FromBody] Topic input)
        {
            var topic = await _capabilityServiceClient.CreateTopic(input.Name, input.Description, id, input.IsPrivate);

            if (topic != null)
            {
                return new ActionResult<Topic>(topic);
            }
            
            return new ActionResult<Topic>(BadRequest());
        }

        [HttpPost("{id}/members", Name = "JoinCapability")]
        public async Task<ActionResult<Member>> JoinCapability([FromRoute] string id, [FromBody] JoinCapabilityInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                return new ActionResult<Member>(BadRequest());
            }

            try
            {
                await _capabilityServiceClient.JoinCapability(
                    capabilityId: id,
                    memberEmail: input.Email
                );

                return new ActionResult<Member>(NoContent());
            }
            catch (AlreadyJoinedException)
            {
                return new ActionResult<Member>(Conflict(new
                {
                    Message = "User is already part of the capability"
                }));
            }
        }

        [HttpDelete("{id}/members/{memberEmail}", Name = "LeaveCapability")]
        public async Task<IActionResult> LeaveCapability([FromRoute] string id, [FromRoute] string memberEmail)
        {
            try
            {
                await _capabilityServiceClient.LeaveCapability(id, memberEmail);
                return NoContent();                
            }
            catch (UnknownCapabilityException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/contexts", Name = "AddContext")]
        public async Task<ActionResult<Capability>> AddContext([FromRoute] string id)
        {
            try
            {
                await _capabilityServiceClient.AddContext(
                    capabilityId: id,
                    contextName: "default"
                );

                return new ActionResult<Capability>(NoContent());
            }
            catch (ContextAlreadyAddedException)
            {
                return new ActionResult<Capability>(Conflict(new
                {
                    Message = "Default context already added to capability"
                }));
            }
        }
    }

    public class AlreadyJoinedException : Exception
    {
    }

    public class ContextAlreadyAddedException : Exception
    {
        
    }

    public class CapabilityValidationException : Exception
    {
        public CapabilityValidationException(string message) : base(message)
        {

        }
    }

    public class UnknownCapabilityException : Exception
    {

    }

    public class JoinCapabilityInput
    {
        public string Email { get; set; }
    }

    public class AddTopicInput
    {
        public string Name { get; set; }
    }
}