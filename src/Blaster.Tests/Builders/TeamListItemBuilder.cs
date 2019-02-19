using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.Builders
{
    public class TeamListItemBuilder
    {
        public Capability Build()
        {
            return new Capability
            {
                Id = "1",
                Name = "team foo",
                Members = new Member[0]
            };
        }
    }
}