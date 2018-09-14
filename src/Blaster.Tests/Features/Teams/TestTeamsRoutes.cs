using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
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
                    .WithService<ITeamService>(new StubTeamService())
                    .Build();

                var response = await client.GetAsync("/api/teams");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_team_through_api_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stubTeam = new TeamListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ITeamService>(new StubTeamService(stubTeam))
                    .Build();

                var dummyContent = "{ }";

                var response = await client.PostAsync("/api/teams", new StringContent(dummyContent, Encoding.UTF8, "application/json"));

                Assert.Equal(
                    expected: HttpStatusCode.Created,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_team_through_api_returns_expected_location_header()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stubTeam = new TeamListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ITeamService>(new StubTeamService(stubTeam))
                    .Build();

                var dummyContent = "{ }";

                var response = await client.PostAsync("/api/teams", new StringContent(dummyContent, Encoding.UTF8, "application/json"));

                Assert.EndsWith(
                    expectedEndString: $"/api/teams/{stubTeam.Id}",
                    actualString: string.Join("", response.Headers.Location.Segments)
                );
            }
        }

        [Fact]
        public async Task get_single_team_returns_expected_status_code_when_no_teams_are_available()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ITeamService>(new StubTeamService())
                    .Build();

                var response = await client.GetAsync("/api/teams/1");

                Assert.Equal(
                    expected: HttpStatusCode.NotFound,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_single_team_returns_expected_status_code_when_team_is_available()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stubTeam = new TeamListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ITeamService>(new StubTeamService(stubTeam))
                    .Build();

                var response = await client.GetAsync($"/api/teams/{stubTeam.Id}");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }
    }
}