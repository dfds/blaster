using Blaster.WebApi.Features.MyServices;
using Xunit;

namespace Blaster.Tests.Features.MyServices
{
    public class UriStringUtilFacts
    {
        [Fact]
        public void AddPath_GIVEN_uri_without_trailing_slash_EXPECT_combined_URI()
        {
            // Arrange
            var baseUri = "http://base/path";

            var path = "relative/path/";

            
            // Act
            var combinedUri = baseUri.AddPath(path);
            
            
            // Assert
            var expectedUri = "http://base/path/relative/path/";
            Assert.Equal(expectedUri, combinedUri);
        }
        
        
        [Fact]
        public void AddPath_GIVEN_path_without_trailing_slash_EXPECT_combined_URI()
        {
            // Arrange
            var baseUri = "http://base/path";

            var path = "/relative/path/";

            
            // Act
            var combinedUri = baseUri.AddPath(path);
            
            
            // Assert
            var expectedUri = "http://base/path/relative/path/";
            Assert.Equal(expectedUri, combinedUri);
        }
    }
}