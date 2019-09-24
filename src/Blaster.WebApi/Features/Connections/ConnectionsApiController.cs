using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Blaster.WebApi.Features.Channels;
using Blaster.WebApi.Features.Channels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Connections
{
    [Route("api/connections")]
    [ApiController]
    public class ConnectionsApiController
    {
        private readonly IHaraldClient _haraldClient;

        public ConnectionsApiController(IHaraldClient haraldClient)
        {
            _haraldClient = haraldClient;
        }

        [HttpGet("", Name = "GetAllConnections")]
        public async Task<ActionResult<ConnectionsResponse>> GetAll(string clientName, string clientType, string clientId, string channelName, string channelType, string channelId)
        {
            var channels = await _haraldClient.GetAllConnections(clientName, clientType, clientId, channelName, channelType, channelId);

            return channels ?? new ConnectionsResponse()
            {
                Items = new Connection[0]
            };
        }
    }
}