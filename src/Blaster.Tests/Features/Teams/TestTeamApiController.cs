using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Teams.Models;
using Xunit;

namespace Blaster.Tests.Features.Teams
{
    public class TestTeamApiController
    {
        [Fact]
        public async Task returns_expected_when_no_teams_are_available()
        {
            var sut = new TeamApiControllerBuilder().Build();
            var result = await sut.GetAll();

            Assert.Empty(result.Value.Items);
        }

        [Fact]
        public async Task returns_expected_when_single_team_is_available()
        {
            var expected = new TeamListItemBuilder().Build();

            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new StubTeamService(expected))
                .Build();

            var result = await sut.GetAll();

            Assert.Equal(
                expected: new[] {expected},
                actual: result.Value.Items
            );
        }

        [Fact]
        public async Task returns_expected_when_multiple_team_are_available()
        {
            var expected = new[]
            {
                new TeamListItemBuilder().Build(),
                new TeamListItemBuilder().Build(),
            };

            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new StubTeamService(expected))
                .Build();

            var result = await sut.GetAll();

            Assert.Equal(
                expected: expected,
                actual: result.Value.Items
            );
        }

        [Fact]
        public async Task returns_expected_when_creating_new_team()
        {
            var expected = new TeamListItemBuilder().Build();

            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new StubTeamService(expected))
                .Build();

            var dummyInput = new TeamInput();

            var result = await sut.CreateTeam(dummyInput);

            Assert.Equal(
                expected: expected,
                actual: result.Value
            );
        }

        [Fact]
        public async Task returns_expected_when_single_team_found_by_id()
        {
            var expected = new TeamListItemBuilder().Build();

            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new StubTeamService(expected))
                .Build();

            var result = await sut.GetById(expected.Id);

            Assert.Equal(
                expected: expected,
                actual: result.Value
            );
        }
    }
}