namespace Blaster.WebApi.Features.Channels.Models
{
    public class ChannelConnectionRequest
    {
        public string SenderId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
    }
}