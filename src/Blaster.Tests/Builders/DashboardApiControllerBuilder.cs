using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.Dashboards;

namespace Blaster.Tests.Builders
{
    public class DashboardApiControllerBuilder
    {
        private IDashboardRepository _dashboardRepository;

        public DashboardApiControllerBuilder()
        {
            _dashboardRepository = Dummy.Of<IDashboardRepository>();
        }

        public DashboardApiControllerBuilder WithDashboardRepository(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
            return this;
        }

        public DashboardApiController Build()
        {
            return new DashboardApiController(_dashboardRepository);
        }
    }
}