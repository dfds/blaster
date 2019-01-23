using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.Teams;

namespace Blaster.Tests.Builders
{
    public class TeamApiControllerBuilder
    {
        private ITeamServiceClient _teamServiceClient;

        public TeamApiControllerBuilder()
        {
            _teamServiceClient = Dummy.Of<ITeamServiceClient>();
        }

        public TeamApiControllerBuilder WithTeamService(ITeamServiceClient teamServiceClient)
        {
            _teamServiceClient = teamServiceClient;
            return this;
        }

        public TeamApiController Build()
        {
            return new TeamApiController(_teamServiceClient);
        }
    }
}