namespace Blaster.WebApi.Features.CommunicationChannels.Models
{
    public class Connection
    {
        public string SenderType { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ChannelType { get; set; }
        public string ChannelName { get; set; }
        public string ChannelId { get; set; }
    }
}