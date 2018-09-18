using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.Helpers;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Features.Teams.Models;
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
                    .WithService<ITeamService>(new StubTeamService(teams: stubTeam))
                    .Build();

                var dummyContent = JsonContent.Empty;

                var response = await client.PostAsync("/api/teams", dummyContent);

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
                    .WithService<ITeamService>(new StubTeamService(teams: stubTeam))
                    .Build();

                var dummyContent = JsonContent.Empty;

                var response = await client.PostAsync("/api/teams", dummyContent);

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
                    .WithService<ITeamService>(new StubTeamService(teams: stubTeam))
                    .Build();

                var response = await client.GetAsync($"/api/teams/{stubTeam.Id}");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_member_to_team_through_api_returns_expected_status_code_on_success()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var dummyUser = new UserBuilder().Build();

                var client = clientBuilder
                    .WithService<ITeamService>(new StubTeamService(user: dummyUser))
                    .Build();

                var dummyContent = new JsonContent(new {UserId = 1});

                var response = await client.PostAsync("/api/teams/1/members", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_member_to_team_through_api_returns_expected_status_code_when_userid_is_missing()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ITeamService>(Dummy.Of<ITeamService>())
                    .Build();

                var dummyContent = JsonContent.Empty;

                var response = await client.PostAsync("/api/teams/1/members", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.BadRequest,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_member_to_team_through_api_returns_expected_status_code_when_member_already_joined()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ITeamService>(new ErroneousTeamService(new AlreadyJoinedException()))
                    .Build();

                var dummyContent = new JsonContent(new {UserId = 1});

                var response = await client.PostAsync("/api/teams/1/members", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.Conflict,
                    actual: response.StatusCode
                );
            }
        }

        public class JsonContent : StringContent
        {
            public JsonContent(object instance) 
                : base(ConvertToJson(instance), Encoding.UTF8, "application/json")
            {

            }

            public static string ConvertToJson(object instance)
            {
                if (instance == null)
                {
                    return "{ }";
                }

                var serializer = new JsonSerializer();
                return serializer.Serialize(instance);
            }

            public static JsonContent Empty => new JsonContent(null);
        }
    }

    public class ErroneousTeamService : ITeamService
    {
        private readonly Exception _error;

        public ErroneousTeamService(Exception error)
        {
            _error = error;
        }

        public Task<TeamListResponse> GetAll()
        {
            throw _error;
        }

        public Task<TeamListItem> CreateTeam(string name, string department)
        {
            throw _error;
        }

        public Task<TeamListItem> GetById(string id)
        {
            throw _error;
        }

        public Task<User> JoinTeam(string teamId, string userId)
        {
            throw _error;
        }
    }

    public class UserBuilder
    {
        private string _id;
        private string _name;
        private string _email;

        public UserBuilder()
        {
            _id = "1";
            _name = "foo";
            _email = "bar";
        }

        public UserBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public UserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Id = _id,
                Name = _name,
                Email = _email
            };
        }
    }
}