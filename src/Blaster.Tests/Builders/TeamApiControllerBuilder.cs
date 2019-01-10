using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.Teams;

namespace Blaster.Tests.Builders
{
    public class TeamApiControllerBuilder
    {
        private ITeamService _teamService;

        public TeamApiControllerBuilder()
        {
            _teamService = Dummy.Of<ITeamService>();
        }

        public TeamApiControllerBuilder WithTeamService(ITeamService teamService)
        {
            _teamService = teamService;
            return this;
        }

        public TeamApiController Build()
        {
            return new TeamApiController(_teamService);
        }
    }
}