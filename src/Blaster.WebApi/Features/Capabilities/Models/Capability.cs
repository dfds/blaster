namespace Blaster.WebApi.Features.Capabilities.Models
{
    public class Capability
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Member[] Members { get; set; }
        public Context[] Contexts { get; set; }
        public Topic[] Topics {get;set;}
    }
}