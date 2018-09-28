namespace Blaster.WebApi.Features.Teams.Models
{
    public class TeamListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public Member[] Members { get; set; }
    }
}