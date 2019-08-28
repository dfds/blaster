using System;
using System.Net;
using System.Net.Http;
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

        [HttpGet(Name = "GetAllCapabilities")]
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

        [HttpPost(Name = "CreateCapability")]
        public async Task<IActionResult> CreateCapability([FromBody] CapabilityInput input)
        {
            Capability capability;
            try
            {
                capability = await _capabilityServiceClient.CreateCapability(input.Name, input.Description);
            } catch (RecoverableUpstreamException tve)
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
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("{id}/commonprefix", Name = "SetTopicCommonPrefix")]
        public async Task<IActionResult> SetTopicCommonPrefix(string id, [FromBody] CapabilityCommonPrefixInput input)
        {
            try
            {
                await _capabilityServiceClient.SetCapabilityTopicCommonPrefix(input.CommonPrefix, id);
                return NoContent();
            }
            catch (CapabilityTopicValidationException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        
        [HttpPost("{id}/topics", Name = "CreateTopic")]
        public async Task<ActionResult<string>> CreateTopic(string id, [FromBody] Topic input)
        {
            try
            {
                await _capabilityServiceClient.CreateTopic(input.Name, input.Description, id, input.IsPrivate);
            }
            catch (HttpRequestException)
            {
                return new ActionResult<string>(BadRequest());
            }
            
            return new ActionResult<string>(NoContent());
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

    
    public class RecoverableUpstreamException : Exception
    {
        public RecoverableUpstreamException(
            HttpStatusCode httpStatusCode, 
            string message
        )
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
        }

        public HttpStatusCode HttpStatusCode { get; }
        public override string Message { get; }
    }
    
    public class CapabilityValidationException : Exception
    {
        public CapabilityValidationException(string message) : base(message)
        {

        }
    }

    public class CapabilityTopicValidationException : Exception
    {
        public CapabilityTopicValidationException(string message) : base(message)
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