using System.Net;
using System.Threading.Tasks;
using Blaster.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Blaster.Tests
{
    public class TestHelloWorldController
    {
        [Fact]
        public async Task get_returns_expected_status_code()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(webHostBuilder);
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: response.StatusCode
            );
        }

        [Fact]
        public async Task get_returns_expected_body()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(webHostBuilder);
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: "hello world",
                actual: await response.Content.ReadAsStringAsync()
            );
        }
    }
}