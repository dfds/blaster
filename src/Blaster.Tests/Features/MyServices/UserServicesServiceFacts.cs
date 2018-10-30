using System.Linq;
using Blaster.WebApi.Features.MyServices;
using DFDS.TeamService.WebApi.Features.UserServices.model;
using Xunit;

namespace Blaster.Tests.Features.MyServices
{
    public class UserServicesServiceFacts
    {
        [Fact]
        public void ReBaseServiceLocations_WILL_rewrite_locations()
        {
            // Arrange
            var service = new ServiceDTO
            {
                Location = "service/location"
            };

            var team = new TeamDTO
            {
                Services = new[] {service}
            };

            var teams = new TeamsDTO
            {
                Items = new[] {team}
            };

            var newBaseUri = "http://base/uri";

            // Act
            teams = UserServicesService.ReBaseServiceLocations(newBaseUri, teams);


            // Assert
            var expectedUri = "http://base/uri/service/location";
            Assert.Equal(
                expectedUri,
                teams.Items.Single().Services.Single().Location
            );
        }
    }
}