using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Xunit;

namespace Blaster.Tests.Features.System
{
    public class TestSystemRoutes
    {
        [Fact]
        public async Task get_health_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder.Build();

                var response = await client.GetAsync("/system/health");
                
                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }
    }
}