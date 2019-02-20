using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Features.Teams.Models;
using Microsoft.AspNetCore.Mvc;
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
                .WithTeamService(new StubCapabilityServiceClient(capabilities: expected))
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
                .WithTeamService(new StubCapabilityServiceClient(capabilities: expected))
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
                .WithTeamService(new StubCapabilityServiceClient(capabilities: expected))
                .Build();

            var dummyInput = new CapabilityInput();

            var result = (CreatedAtRouteResult)await sut.CreateTeam(dummyInput);

            Assert.Equal(
                expected: expected,
                actual: result.Value
            );
        }

        [Fact]
        public async Task returns_badrequest_when_creating_new_team_with_invalid_name()
        {
            var expected = new TeamListItemBuilder().Build();

            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new ErroneousCapabilityServiceClient(new TeamValidationException("booo")))
                .Build();

            var dummyInput = new CapabilityInput();

            var result = await sut.CreateTeam(dummyInput);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task returns_expected_when_single_team_found_by_id()
        {
            var expected = new TeamListItemBuilder().Build();

            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new StubCapabilityServiceClient(capabilities: expected))
                .Build();

            var result = await sut.GetById(expected.Id);

            Assert.Equal(
                expected: expected,
                actual: result.Value
            );
        }

        //[Fact]
        //public async Task returns_expected_when_user_joins_a_team()
        //{
        //    var expected = new UserBuilder().Build();

        //    var sut = new TeamApiControllerBuilder()
        //        .WithTeamService(new StubTeamService(member: expected))
        //        .Build();

        //    var stubTeamId = "foo";

        //    var result = await sut.JoinTeam(stubTeamId, new JoinTeamInput { Email = expected.Id });

        //    Assert.Equal(expected, result.Value);
        //}

        [Fact]
        public async Task returns_expected_when_user_joins_a_team_and_user_has_already_joined()
        {
            var sut = new TeamApiControllerBuilder()
                .WithTeamService(new ErroneousCapabilityServiceClient(new AlreadyJoinedException()))
                .Build();

            var result = await sut.JoinTeam("foo", new JoinCapabilityInput {Email = "bar"});
            
            Assert.Null(result.Value);
        }
    }
}