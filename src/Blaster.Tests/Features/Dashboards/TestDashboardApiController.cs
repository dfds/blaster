using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.Helpers;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Dashboards.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Blaster.Tests.Features.Dashboards
{
    public class TestDashboardApiController
    {
        [Fact]
        public async Task get_returns_expected_when_no_dashboards_are_available()
        {
            var sut = new DashboardApiControllerBuilder().Build();
            var result = await sut.GetAll();

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
                .WithDashboardService(new StubDashboardService(listResult: expectedDashboards))
                .Build();

            var result = await sut.GetAll();
            
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
            var result = await sut.GetById(nonExistingId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task get_by_id_returns_expected_when_dashboard_is_available()
        {
            var expected = new DashboardDetailItemBuilder().Build();

            var sut = new DashboardApiControllerBuilder()
                .WithDashboardService(new StubDashboardService(singleResult: expected))
                .Build();

            var result = await sut.GetById(expected.Id);

            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async Task post_returns_expected()
        {
            var expected = new DashboardDetailItemBuilder().Build();

            var sut = new DashboardApiControllerBuilder()
                .WithDashboardService(new StubDashboardService(singleResult:expected))
                .Build();

            var result = await sut.Post(new DashboardInput
            {
                Team = expected.Team,
                Name = expected.Name,
                Content = expected.Content
            });
            
            Assert.Equal(
                expected: expected,
                actual: result.Value,
                comparer: new PropertiesComparer<DashboardDetailItem>(
                        x => x.Team,
                        x => x.Name,
                        x => x.Content
                    )
            );
        }
    }
}