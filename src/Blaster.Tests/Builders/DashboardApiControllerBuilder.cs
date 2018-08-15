using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.Dashboards;

namespace Blaster.Tests.Builders
{
    public class DashboardApiControllerBuilder
    {
        private IDashboardService _dashboardService;

        public DashboardApiControllerBuilder()
        {
            _dashboardService = Dummy.Of<IDashboardService>();
        }

        public DashboardApiControllerBuilder WithDashboardService(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            return this;
        }

        public DashboardApiController Build()
        {
            return new DashboardApiController(_dashboardService);
        }
    }
}