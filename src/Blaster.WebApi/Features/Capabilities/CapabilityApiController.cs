using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilities
{
    [Route("api/capabilities")]
    [ForwardHeader]
    [ApiController]
    public class CapabilityApiController : ControllerBase
    {
        private readonly ICapabilityServiceClient _capabilityServiceClient;

        public CapabilityApiController(ICapabilityServiceClient capabilityServiceClient)
        {
            _capabilityServiceClient = capabilityServiceClient;
        }
        public void ForwardHeaders()
        {
	        ForwardHeader.ForwardMsal(
		        request: Request, 
		        client: _capabilityServiceClient);
        }

        [HttpGet(Name = "GetAllCapabilities")]
        public async Task<ActionResult<CapabilitiesResponse>> GetAll()
        {
	        try
	        {
		        var capabilities = await _capabilityServiceClient.GetAll();

		        return capabilities ?? new CapabilitiesResponse {Items = new Capability[0]};
	        }
	        catch (UnauthorizedException)
	        {
		        return Unauthorized();
	        }
        }

        [HttpGet("{id}", Name = "GetCapabilityById")]
        public async Task<ActionResult<Capability>> GetById(string id)
        {
	        try
	        {
		        var capability = await _capabilityServiceClient.GetById(id);

		        if (capability != null)
		        {
			        return new ActionResult<Capability>(capability);
		        }

		        return new ActionResult<Capability>(NotFound());
	        }
	        catch (UnauthorizedException)
	        {
		        return Unauthorized();
	        }
        }

        [HttpPost(Name = "CreateCapability")]
        public async Task<IActionResult> CreateCapability([FromBody] CapabilityInput input)
        {
            Capability capability;
            try
            {
	            capability = await _capabilityServiceClient.CreateCapability(input.Name, input.Description);
            }
            catch (UnauthorizedException)
            {
	            return Unauthorized();
            }
            catch (RecoverableUpstreamException tve)
            {
	            return new ObjectResult(new {tve.Message}) { StatusCode = (int)tve.HttpStatusCode };
            }
            
            
            var createdAtRouteResultConverter = new CreatedAtRouteResultConverter<Capability>(
                routeName: "GetCapabilityById",
                routeValues: new { id = capability.Id },
                value: capability
            );

            return createdAtRouteResultConverter.Convert();
        }

        // This method on purpose only updates "description" at the moment. Public access to updating all of a Capability is yet to be decided.
        [HttpPut("{id}", Name = "UpdateCapability")]
        public async Task<IActionResult> UpdateCapability(string id, [FromBody] CapabilityInput input)
        {
            try
            {
                var currentCapability = await _capabilityServiceClient.GetById(id);
                await _capabilityServiceClient.UpdateCapability(id, currentCapability.Name, input.Description);
            }
            catch (UnauthorizedException)
            {
	            return Unauthorized();
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteCapability")]
        public async Task<IActionResult> DeleteCapability(string id)
        {
            try
            {
                await _capabilityServiceClient.DeleteCapability(id);
            }
            catch (UnauthorizedException)
            {
	            return Unauthorized();
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
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
            catch (UnauthorizedException)
            {
	            return Unauthorized();
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
            catch (UnauthorizedException)
            {
	            return Unauthorized();
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
            catch (UnauthorizedException)
            {
	            return Unauthorized();
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

 

    public class JoinCapabilityInput
    {
        public string Email { get; set; }
    }

    public class AddTopicInput
    {
        public string Name { get; set; }
    }
}
