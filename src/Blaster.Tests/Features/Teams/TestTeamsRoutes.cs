using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.Features.System;
using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams;
using Xunit;

namespace Blaster.Tests.Features.Teams
{
    public class TestTeamsRoutes
    {
        [Fact]
        public async Task get_front_page_for_teams_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder.Build();

                var response = await client.GetAsync("/teams");
                
                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_teams_from_api_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICognitoService>(new StubCognitoService())
                    .Build();

                var response = await client.GetAsync("/api/teams");
                
                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }
    }

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
                .WithCognitoService(new StubCognitoService(expected))
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
                .WithCognitoService(new StubCognitoService(expected))
                .Build();

            var result = await sut.GetAll();

            Assert.Equal(
                expected: expected,
                actual: result.Value.Items
            );
        }
    }

    public class TeamListItemBuilder
    {
        public TeamListItem Build()
        {
            return new TeamListItem();
        }
    }

    public class TeamApiControllerBuilder
    {
        private ICognitoService _cognitoService;

        public TeamApiControllerBuilder()
        {
            _cognitoService = Dummy.Of<ICognitoService>();
        }

        public TeamApiControllerBuilder WithCognitoService(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
            return this;
        }

        public TeamApiController Build()
        {
            return new TeamApiController(_cognitoService);
        }
    }
}