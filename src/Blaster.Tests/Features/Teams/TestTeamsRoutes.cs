using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
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
    }
}