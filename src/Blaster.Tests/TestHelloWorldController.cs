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

            var result = response.StatusCode;

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: result
            );
        }

        [Fact]
        public async Task get_returns_expected_body()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(webHostBuilder);

            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal(
                expected: "hello world",
                actual: result
            );
        }
    }
}