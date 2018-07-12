using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi;
using Xunit;

namespace Blaster.Tests
{
    public class TestHelloWorldController
    {
        [Fact]
        public async Task get_returns_expected_status_code()
        {
            var client = new HttpClientBuilder().Build();
            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: response.StatusCode
            );
        }

        [Fact]
        public async Task get_returns_expected_body()
        {
            var client = new HttpClientBuilder().Build();
            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: "hello world",
                actual: await response.Content.ReadAsStringAsync()
            );
        }

        [Fact]
        public async Task get_returns_expected_status_code_when_required_api_key_is_omitted()
        {
            var client = new HttpClientBuilder()
                .WithApiKey(null)
                .Build();

            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: HttpStatusCode.Forbidden,
                actual: response.StatusCode
            );
        }

        [Fact]
        public async Task get_returns_expected_status_code_when_required_api_key_is_valid()
        {
            var validApiKey = "foo";

            var client = new HttpClientBuilder()
                .WithApiKey(validApiKey)
                .Build();

            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: HttpStatusCode.OK,
                actual: response.StatusCode
            );
        }

        [Fact]
        public async Task get_returns_expected_status_code_when_required_api_key_is_invalid()
        {
            var client = new HttpClientBuilder()
                .WithService(typeof(IApiKeyValidator), new StubApiKeyValidator(isValid: false))
                .Build();

            var response = await client.GetAsync("/");

            Assert.Equal(
                expected: HttpStatusCode.Forbidden,
                actual: response.StatusCode
            );
        }
    }
}