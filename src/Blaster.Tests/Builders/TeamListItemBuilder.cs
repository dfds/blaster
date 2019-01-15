using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.Builders
{
    public class TeamListItemBuilder
    {
        public TeamListItem Build()
        {
            return new TeamListItem
            {
                Id = "1",
                Name = "team foo",
                Members = new Member[0]
            };
        }
    }
}