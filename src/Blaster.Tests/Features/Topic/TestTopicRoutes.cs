using System.Net;
using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.Helpers;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Topic;
using Xunit;

namespace Blaster.Tests.Features.Topic
{
    public class TestTopicRoutes
    {
        [Fact]
        public async Task get_front_page_for_topics_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder.Build();

                var response = await client.GetAsync("/topics");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task get_topics_from_api_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var client = clientBuilder
                    .WithService<ITopicClient>(new TopicClientStub())
                    .Build();

                var response = await client.GetAsync("/api/topics");

                Assert.Equal(
                    expected: HttpStatusCode.OK,
                    actual: response.StatusCode
                );
            }
        }

        [Fact]
        public async Task post_topic_through_api_returns_expected_status_code()
        {
            using (var clientBuilder = new HttpClientBuilder())
            {
                var stub = new CapabilityListItemBuilder().Build();

                var client = clientBuilder
                    .WithService<ITopicClient>(new TopicClientStub())
                    .Build();

                var dummyContent = new JsonContent(new CreateTopicRequest {Name = "dummyTopic"});

                var response = await client.PostAsync("/api/topics", dummyContent);

                Assert.Equal(
                    expected: HttpStatusCode.NoContent,
                    actual: response.StatusCode
                );
            }
        }
    }
}