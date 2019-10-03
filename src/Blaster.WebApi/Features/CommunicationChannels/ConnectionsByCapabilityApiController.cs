using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Channels;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.CommunicationChannels
{
    [ApiController]
    public class ConnectionsApiController : ControllerBase
    {
        private readonly IHaraldClient _haraldClient;

        public ConnectionsApiController(IHaraldClient haraldClient)
        {
            _haraldClient = haraldClient;
        }

        
        [HttpGet("api/capabilities/{id}/connections", Name = "GetChannelsByCapabilityId")]
        public async Task<ActionResult<ConnectionsResponse>> GetChannelsById(string id)
        {
            var connectionsResponse = await _haraldClient.GetConnectionsByCapabilityId(id);

            return new ActionResult<ConnectionsResponse>(connectionsResponse);
        }
    }
}