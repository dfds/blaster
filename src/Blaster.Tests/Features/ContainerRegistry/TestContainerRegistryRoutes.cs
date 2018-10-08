using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Xunit;

namespace Blaster.Tests.Features.ContainerRegistry
{
    public class TestContainerRegistryRoutes
    {
        [Fact]
        public async Task get_front_page_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder.Build();

                var response = await client.GetAsync("/containerregistry");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        //[Fact]
        //public async Task get_teams_from_api_returns_expected_status_code()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var client = clientBuilder.Build();

        //        var response = await client.GetAsync("/api/containerregistries");

        //        Assert.Equal(
        //            expected: HttpStatusCode.OK,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //[Fact]
        //public async Task post_team_through_api_returns_expected_status_code()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var stubTeam = new TeamListItemBuilder().Build();

        //        var client = clientBuilder
        //            .WithService<ITeamService>(new StubTeamService(teams: stubTeam))
        //            .Build();

        //        var dummyContent = JsonContent.Empty;

        //        var response = await client.PostAsync("/api/teams", dummyContent);

        //        Assert.Equal(
        //            expected: HttpStatusCode.Created,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //[Fact]
        //public async Task post_team_through_api_returns_expected_location_header()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var stubTeam = new TeamListItemBuilder().Build();

        //        var client = clientBuilder
        //            .WithService<ITeamService>(new StubTeamService(teams: stubTeam))
        //            .Build();

        //        var dummyContent = JsonContent.Empty;

        //        var response = await client.PostAsync("/api/teams", dummyContent);

        //        Assert.EndsWith(
        //            expectedEndString: $"/api/teams/{stubTeam.Id}",
        //            actualString: string.Join("", response.Headers.Location.Segments)
        //        );
        //    }
        //}

        //[Fact]
        //public async Task get_single_team_returns_expected_status_code_when_no_teams_are_available()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var client = clientBuilder
        //            .WithService<ITeamService>(new StubTeamService())
        //            .Build();

        //        var response = await client.GetAsync("/api/teams/1");

        //        Assert.Equal(
        //            expected: HttpStatusCode.NotFound,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //[Fact]
        //public async Task get_single_team_returns_expected_status_code_when_team_is_available()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var stubTeam = new TeamListItemBuilder().Build();

        //        var client = clientBuilder
        //            .WithService<ITeamService>(new StubTeamService(teams: stubTeam))
        //            .Build();

        //        var response = await client.GetAsync($"/api/teams/{stubTeam.Id}");

        //        Assert.Equal(
        //            expected: HttpStatusCode.OK,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //[Fact]
        //public async Task post_member_to_team_through_api_returns_expected_status_code_on_success()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var dummyUser = new UserBuilder().Build();

        //        var client = clientBuilder
        //            .WithService<ITeamService>(new StubTeamService(member: dummyUser))
        //            .Build();

        //        var dummyContent = new JsonContent(new {UserId = 1});

        //        var response = await client.PostAsync("/api/teams/1/members", dummyContent);

        //        Assert.Equal(
        //            expected: HttpStatusCode.OK,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //[Fact]
        //public async Task post_member_to_team_through_api_returns_expected_status_code_when_userid_is_missing()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var client = clientBuilder
        //            .WithService<ITeamService>(Dummy.Of<ITeamService>())
        //            .Build();

        //        var dummyContent = JsonContent.Empty;

        //        var response = await client.PostAsync("/api/teams/1/members", dummyContent);

        //        Assert.Equal(
        //            expected: HttpStatusCode.BadRequest,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //[Fact]
        //public async Task post_member_to_team_through_api_returns_expected_status_code_when_member_already_joined()
        //{
        //    using (var clientBuilder = new HttpClientBuilder())
        //    {
        //        var client = clientBuilder
        //            .WithService<ITeamService>(new ErroneousTeamService(new AlreadyJoinedException()))
        //            .Build();

        //        var dummyContent = new JsonContent(new {UserId = 1});

        //        var response = await client.PostAsync("/api/teams/1/members", dummyContent);

        //        Assert.Equal(
        //            expected: HttpStatusCode.Conflict,
        //            actual: response.StatusCode
        //        );
        //    }
        //}

        //public class JsonContent : StringContent
        //{
        //    public JsonContent(object instance) 
        //        : base(ConvertToJson(instance), Encoding.UTF8, "application/json")
        //    {

        //    }

        //    public static string ConvertToJson(object instance)
        //    {
        //        if (instance == null)
        //        {
        //            return "{ }";
        //        }

        //        var serializer = new JsonSerializer();
        //        return serializer.Serialize(instance);
        //    }

        //    public static JsonContent Empty => new JsonContent(null);
        //}
    }
}