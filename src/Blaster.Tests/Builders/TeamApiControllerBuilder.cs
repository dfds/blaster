using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.Teams;

namespace Blaster.Tests.Builders
{
    public class TeamApiControllerBuilder
    {
        private ICapabilityServiceClient _capabilityServiceClient;

        public TeamApiControllerBuilder()
        {
            _capabilityServiceClient = Dummy.Of<ICapabilityServiceClient>();
        }

        public TeamApiControllerBuilder WithTeamService(ICapabilityServiceClient capabilityServiceClient)
        {
            _capabilityServiceClient = capabilityServiceClient;
            return this;
        }

        public TeamApiController Build()
        {
            return new TeamApiController(_capabilityServiceClient);
        }
    }
}