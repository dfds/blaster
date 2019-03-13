using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.Helpers;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Capabilities;
using Xunit;

namespace Blaster.Tests.Features.Capabilities
{
    public class TestCapabilityRoutes
    {
        [Fact]
        public async Task get_front_page_for_capabilities_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder.Build();

                var response = await client.GetAsync("/capabilities");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_capabilities_from_api_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient())
                    .Build();

                var response = await client.GetAsync("/api/capabilities");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_capability_through_api_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stub = new CapabilityListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient(capabilities: stub))
                    .Build();

                var dummyContent = JsonContent.Empty;

                var response = await client.PostAsync("/api/capabilities", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.Created,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_capability_through_api_returns_expected_location_header()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stub = new CapabilityListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient(capabilities: stub))
                    .Build();

                var dummyContent = JsonContent.Empty;

                var response = await client.PostAsync("/api/capabilities", dummyContent);

                Assert.EndsWith(
                    expectedEndString: $"/api/capabilities/{stub.Id}",
                    actualString: string.Join("", response.Headers.Location.Segments)
                );
            }
        }

        [Fact]
        public async Task get_single_capability_returns_expected_status_code_when_no_capabilities_are_available()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient())
                    .Build();

                var response = await client.GetAsync("/api/capabilities/1");

                Assert.Equal(
                    expected: HttpStatusCode.NotFound,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_single_capability_returns_expected_status_code_when_capability_is_available()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stub = new CapabilityListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient(capabilities: stub))
                    .Build();

                var response = await client.GetAsync($"/api/capabilities/{stub.Id}");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_member_to_capability_through_api_returns_expected_status_code_on_success()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var dummyUser = new UserBuilder().Build();

                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient(member: dummyUser))
                    .Build();

                var dummyContent = new JsonContent(new {Email = "foo@bar.com"});

                var response = await client.PostAsync("/api/capabilities/1/members", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.NoContent,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_member_to_capability_through_api_returns_expected_status_code_when_userid_is_missing()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(Dummy.Of<ICapabilityServiceClient>())
                    .Build();

                var dummyContent = JsonContent.Empty;

                var response = await client.PostAsync("/api/capabilities/1/members", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.BadRequest,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_member_to_capability_through_api_returns_expected_status_code_when_member_already_joined()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new ErroneousCapabilityServiceClient(new AlreadyJoinedException()))
                    .Build();

                var dummyContent = new JsonContent(new {Email = "foo@bar.com"});

                var response = await client.PostAsync("/api/capabilities/1/members", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.Conflict,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task delete_member_from_capability_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new StubCapabilityServiceClient())
                    .Build();

                var response = await client.DeleteAsync("/api/capabilities/1/members/foo@bar.com");

                Assert.Equal(
                    expected: HttpStatusCode.NoContent,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task delete_member_from_capability_returns_expected_status_code_when_capability_does_not_exist()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ICapabilityServiceClient>(new ErroneousCapabilityServiceClient(new UnknownCapabilityException()))
                    .Build();

                var response = await client.DeleteAsync("/api/capabilities/1/members/foo@bar.com");

                Assert.Equal(
                    expected: HttpStatusCode.NotFound,
                    actual: response.StatusCode
                );
            }
        }
    }
}