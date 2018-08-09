using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Blaster.Tests
{
    public class TestDashboardApiController
    {
        [Fact]
        public async Task get_returns_expected_when_no_dashboards_are_available()
        {
            var sut = new DashboardApiControllerBuilder().Build();
            var result = await sut.Get();

            Assert.Empty(result.Value.Items);
        }

        [Fact]
        public async Task get_returns_expected_when_multiple_namespaces_are_available()
        {
            var expectedDashboards = new[]
            {
                new DashboardListItemBuilder().Build(),
                new DashboardListItemBuilder().Build(),
            };

            var sut = new DashboardApiControllerBuilder()
                .WithDashboardRepository(new StubDashboardRepository(listResult: expectedDashboards))
                .Build();

            var result = await sut.Get();

            Assert.Equal(
                expected: expectedDashboards,
                actual: result.Value.Items
            );
        }

        [Fact]
        public async Task get_by_id_returns_expected_when_no_dashboards_are_available()
        {
            var sut = new DashboardApiControllerBuilder().Build();

            var nonExistingId = "foo";
            var result = await sut.Get(nonExistingId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task get_by_id_returns_expected_when_dashboard_is_available()
        {
            var expected = new DashboardDetailItemBuilder().Build();

            var sut = new DashboardApiControllerBuilder()
                .WithDashboardRepository(new StubDashboardRepository(singleResult: expected))
                .Build();

            var result = await sut.Get(expected.Id);

            Assert.Equal(result.Value, expected);
        }
    }
}