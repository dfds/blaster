using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.Builders
{
    public class TeamListItemBuilder
    {
        public Team Build()
        {
            return new Team
            {
                Id = "1",
                Name = "team foo",
                Members = new Member[0]
            };
        }
    }
}