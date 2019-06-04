using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.Tests.Builders
{
    public class CapabilityListItemBuilder
    {
        public Capability Build()
        {
            return new Capability
            {
                Id = "1",
                Name = "capability foo",
                Description = "description foo",
                Members = new Member[0],
                Contexts = new Context[0],
                Topics = new Topic[0]
            };
        }
    }
}