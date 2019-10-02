using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Channels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Channels
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelApiController
    {
        private readonly IHaraldClient _haraldClient;

        public ChannelApiController(IHaraldClient haraldClient)
        {
            _haraldClient = haraldClient;
        }

        [HttpPost("join", Name = "JoinChannel")]
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
            catch (HttpRequestException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
        
        [HttpDelete("leave", Name = "LeaveChannel")]
        public async Task<IActionResult> Leave([FromBody] ChannelConnectionRequest channelConnectionRequest)
        {
            try
            {
                await _haraldClient.LeaveChannel(channelId: channelConnectionRequest.ChannelId, channelType: channelConnectionRequest.ChannelType, 
                    clientId: channelConnectionRequest.ClientId, clientType: channelConnectionRequest.ClientType);
            }
            catch (HttpRequestException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
    }
}