using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilities
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsApiController
    {
        private readonly IHaraldClient _haraldClient;

        public ChannelsApiController(IHaraldClient haraldClient)
        {
            _haraldClient = haraldClient;
        }

        [HttpGet("", Name = "GetAllChannels")]
        public async Task<ActionResult<ChannelsResponse>> GetAll()
        {
            var channels = await _haraldClient.GetAllChannels();

            return channels ?? new ChannelsResponse
            {
                Items = new Channel[0]
            };
        }
    }
}