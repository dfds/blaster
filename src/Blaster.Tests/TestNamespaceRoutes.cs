using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Namespaces;
using Xunit;

namespace Blaster.Tests
{
    public class TestNamespaceRoutes
    {
        [Fact]
        public async Task get_returns_expected_status_code()
        {
            var client = new HttpClientBuilder()
                .WithService(typeof(INamespaceRepository), new StubNamespaceRepository())
                .Build();

            var response = await client.GetAsync("/api/namespaces");

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: response.StatusCode
            );
        }
    }
}