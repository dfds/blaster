namespace Blaster.WebApi.Features.Teams.Models
{
    public class Capability
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Member[] Members { get; set; }
    }
}