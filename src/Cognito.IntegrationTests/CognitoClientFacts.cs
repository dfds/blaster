using System.Threading.Tasks;
using Cognito.WebApi;
using Shouldly;
using Xunit;

namespace Cognito.IntegrationTests
{
    public class CognitoClientFacts
    {
        [Fact]
        public async Task CanIndicateNoConnection()
        {
            // Arrange
            var client = new CognitoClient(null,null);

            
            // Act
            var isAlive = await client.IsAlive();
            
           
            // Assert
            isAlive.ShouldBeFalse();
        }
        
        
        [Fact]
        public async Task CanIndicateConnection()
        {
            // Arrange
            var client = CognitoClientFactory.CreateFromGetEnvironmentVariables();

            
            // Act
            var isAlive = await client.IsAlive();
            
           
            // Assert
            isAlive.ShouldBeTrue();
        }
    }
}