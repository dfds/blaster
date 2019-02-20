using Blaster.Tests.Helpers;
using Blaster.WebApi.Features.Capabilities;

namespace Blaster.Tests.Builders
{
    public class CapabilityApiControllerBuilder
    {
        private ICapabilityServiceClient _capabilityServiceClient;

        public CapabilityApiControllerBuilder()
        {
            _capabilityServiceClient = Dummy.Of<ICapabilityServiceClient>();
        }

        public CapabilityApiControllerBuilder WithCapabilityService(ICapabilityServiceClient capabilityServiceClient)
        {
            _capabilityServiceClient = capabilityServiceClient;
            return this;
        }

        public CapabilityApiController Build()
        {
            return new CapabilityApiController(_capabilityServiceClient);
        }
    }
}