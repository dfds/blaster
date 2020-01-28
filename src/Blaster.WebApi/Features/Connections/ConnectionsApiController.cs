using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Channels;
using Blaster.WebApi.Features.Channels.Models;
using Blaster.WebApi.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Blaster.WebApi.Features.Connections
{
    [Route("api/connections")]
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

        [HttpGet("", Name = "GetAllConnections")]
        public async Task<ActionResult<ConnectionsResponse>> GetAll(string clientName, string clientType, string clientId, string channelName, string channelType, string channelId)
        {
	        try
	        {
		        var channels = await _haraldClient.GetAllConnections(channelId: channelId, 
			        channelName: channelName,
			        channelType: channelType,
			        clientId: clientId,
			        clientName: clientName,
			        clientType: clientType);

		        return channels ?? new ConnectionsResponse()
		        {
			        Items = new Connection[0]
		        };
	        }
	        catch (UnauthorizedException)
	        {
		        return Unauthorized();
	        }
        }
        
        [HttpPost("", Name = "JoinChannelConnection")]
        public async Task<IActionResult> Join([FromBody] ChannelConnectionRequest channelConnectionRequest)
        {
            try
            {
                await _haraldClient.JoinChannel(channelId: channelConnectionRequest.ChannelId, 
                    channelName: channelConnectionRequest.ChannelName,
                    channelType: channelConnectionRequest.ChannelType,
                    clientId: channelConnectionRequest.ClientId,
                    clientName: channelConnectionRequest.ClientName,
                    clientType: channelConnectionRequest.ClientType);
            }
            catch (UnauthorizedException)
            {
	            return Unauthorized();
            }
            catch (HttpRequestException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
        
        [HttpDelete("", Name = "LeaveChannelConnection")]
        public async Task<IActionResult> Leave(string clientType, string clientId, string channelType, string channelId)
        {
            try
            {
                await _haraldClient.LeaveChannel(channelId: channelId, channelType: channelType, clientId: clientId, clientType: clientType);
            }
            catch (UnauthorizedException)
            {
	            return Unauthorized();
            }
            catch (HttpRequestException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
    }
}
