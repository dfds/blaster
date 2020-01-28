using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Channels;
using Blaster.WebApi.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.CommunicationChannels
{
	[Route("cbca")]
	[ForwardHeader]
    [ApiController]
    public class ConnectionsApiController : ControllerBase
    {
        private readonly IHaraldClient _haraldClient;

        public ConnectionsApiController(IHaraldClient haraldClient)
        {
            _haraldClient = haraldClient;
        }
        public void ForwardHeaders()
        {
	        ForwardHeader.ForwardMsal(
		        request: Request, 
		        client: _haraldClient);
        }
        
        [HttpGet("/api/capabilities/{id}/connections", Name = "GetChannelsByCapabilityId")]
        public async Task<ActionResult<ConnectionsResponse>> GetChannelsById(string id)
        {
	        try
	        {
		        var connectionsResponse = await _haraldClient.GetConnectionsByCapabilityId(id);

		        return new ActionResult<ConnectionsResponse>(connectionsResponse);
	        }
	        catch (UnauthorizedException ex)
	        {
		        return Unauthorized();
	        }
        }
    }
}
