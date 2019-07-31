namespace Blaster.WebApi.Features.Topics.models
{
    public class Topic
    {
        public string Id { get; set; }
        public string CapabilityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Public { get; set; }
    }
}