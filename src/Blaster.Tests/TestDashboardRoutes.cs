using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Dashboards;
using Xunit;

namespace Blaster.Tests
{
    public class TestDashboardRoutes
    {
        [Fact]
        public async Task get_returns_expected_status_code()
        {
            var client = new HttpClientBuilder()
                .WithService(typeof(IDashboardRepository), new StubDashboardRepository())
                .Build();

            var response = await client.GetAsync("/api/dashboards");

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: response.StatusCode
            );
        }

        [Fact]
        public async Task get_by_id_returns_expected_status_code_when_dashboards_are_available()
        {
            var stubDashboard = new DashboardDetailItemBuilder().Build();

            var client = new HttpClientBuilder()
                .WithService(typeof(IDashboardRepository), new StubDashboardRepository(singleResult: stubDashboard))
                .Build();

            var response = await client.GetAsync($"/api/dashboards/{stubDashboard.Id}");

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: response.StatusCode
            );
        }

        [Fact]
        public async Task get_by_id_returns_expected_status_code_when_dashboard_is_NOT_available()
        {
            var client = new HttpClientBuilder()
                .WithService(typeof(IDashboardRepository), new StubDashboardRepository())
                .Build();

            var nonExistingId = "foo";
            var response = await client.GetAsync($"/api/dashboards/{nonExistingId}");

            Assert.Equal(
                expected: HttpStatusCode.NotFound,
                actual: response.StatusCode
            );
        }
    }
}