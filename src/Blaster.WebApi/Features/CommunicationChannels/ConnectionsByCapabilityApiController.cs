using System.Threading.Tasks;
using Blaster.WebApi.Features.CommunicationChannels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.CommunicationChannels
{
    [ApiController]
    public class ConnectionsApiController : ControllerBase
    {
        private readonly IHaraldServiceClient _haraldServiceClient;

        public ConnectionsApiController(IHaraldServiceClient haraldServiceClient)
        {
            _haraldServiceClient = haraldServiceClient;
        }

        
        [HttpGet("api/capabilities/{id}/connections", Name = "GetChannelsByCapabilityId")]
        public async Task<ActionResult<ConnectionsResponse>> GetChannelsById(string id)
        {
            var connectionsResponse = await _haraldServiceClient.GetConnectionsByCapabilityIdAsync(id);

            return new ActionResult<ConnectionsResponse>(connectionsResponse);
        }
    }
}