using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Dashboards;
using Xunit;

namespace Blaster.Tests.Features.Dashboards
{
    public class TestDashboardRoutes
    {
        [Fact]
        public async Task get_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<IDashboardService>(new StubDashboardService())
                    .Build();

                var response = await client.GetAsync("/api/dashboards");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_website_does_not_require_apikey()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithApiKey(null)
                    .Build();

                var response = await client.GetAsync("/dashboards");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_by_id_returns_expected_status_code_when_dashboards_are_available()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stubDashboard = new DashboardDetailItemBuilder().Build();

                var client = clientBuilder
                    .WithService<IDashboardService>(new StubDashboardService(singleResult: stubDashboard))
                    .Build();

                var response = await client.GetAsync($"/api/dashboards/{stubDashboard.Id}");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_by_id_returns_expected_status_code_when_dashboard_is_NOT_available()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<IDashboardService>(new StubDashboardService())
                    .Build();

                var nonExistingId = "foo";
                var response = await client.GetAsync($"/api/dashboards/{nonExistingId}");

                Assert.Equal(
                    expected: HttpStatusCode.NotFound,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var dummyDashboard = new DashboardDetailItemBuilder().Build();

                var client = clientBuilder
                    .WithService<IDashboardService>(new StubDashboardService(singleResult: dummyDashboard))
                    .Build();

                var dummyRequestBody = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/dashboards", dummyRequestBody);

                Assert.Equal(
                    expected: HttpStatusCode.Created,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_returns_expected_location_header()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stubDashboard = new DashboardDetailItemBuilder().Build();

                var client = clientBuilder
                    .WithService<IDashboardService>(new StubDashboardService(singleResult: stubDashboard))
                    .Build();

                var dummyRequestBody = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/dashboards", dummyRequestBody);

                Assert.EndsWith(
                    expectedEndString: $"/api/dashboards/{stubDashboard.Id}",
                    actualString: string.Join("", response.Headers.Location.Segments)
                );
            }
        }
    }
}