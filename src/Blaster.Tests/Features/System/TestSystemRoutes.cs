using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams;
using Xunit;
using NotImplementedException = System.NotImplementedException;

namespace Blaster.Tests.Features.System
{
    public class TestSystemRoutes
    {
        [Fact]
        public async Task get_health_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICognitoService>(new StubCognitoService())
                    .Build();

                var response = await client.GetAsync("/system/health");
                
                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }
    }

    public class StubCognitoService : ICognitoService
    {
        private readonly TeamListItem[] _teams;

        public StubCognitoService(params TeamListItem[] teams)
        {
            _teams = teams;
        }

        public Task<string> SayHello()
        {
            return Task.FromResult("foo");
        }

        public Task<TeamListResponse> GetAll()
        {
            return Task.FromResult(new TeamListResponse
            {
                Items = _teams,
            });
        }

        public Task<AwsConsoleLinkResponse> GetAwsConsoleLink(string idToken)
        {
            return Task.FromResult(new AwsConsoleLinkResponse
            {
                AbsoluteUrl = "https://aws.link"
            });
        }
    }
}