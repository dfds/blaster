using System.Threading.Tasks;
using Cognito.WebApi.Controllers;
using Xunit;

namespace Cognito.Tests
{
    public class TestAwsConsoleController
    {
        [Fact]
        public async Task get_console_link_returns_url()
        {
            var consoleBuilder = new Moq.Mock<IAwsConsoleLinkBuilder>();

            var sut = new AwsConsoleController(consoleBuilder.Object);
            var tokenId = "myFancyToken";
            var result = await sut.ConstructLink(tokenId);

            Assert.NotNull(result.Value.AbsoluteUrl);
        }
    }
}