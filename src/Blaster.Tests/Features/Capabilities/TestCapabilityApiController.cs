using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Blaster.Tests.Features.Capabilities
{
    public class TestCapabilityApiController
    {
        [Fact]
        public async Task returns_expected_when_no_capabilities_are_available()
        {
            var sut = new CapabilityApiControllerBuilder().Build();
            var result = await sut.GetAll();

            Assert.Empty(result.Value.Items);
        }

        [Fact]
        public async Task returns_expected_when_single_capability_is_available()
        {
            var expected = new CapabilityListItemBuilder().Build();

            var sut = new CapabilityApiControllerBuilder()
                .WithCapabilityService(new StubCapabilityServiceClient(capabilities: expected))
                .Build();

            var result = await sut.GetAll();

            Assert.Equal(
                expected: new[] {expected},
                actual: result.Value.Items
            );
        }

        [Fact]
        public async Task returns_expected_when_multiple_capability_are_available()
        {
            var expected = new[]
            {
                new CapabilityListItemBuilder().Build(),
                new CapabilityListItemBuilder().Build(),
            };

            var sut = new CapabilityApiControllerBuilder()
                .WithCapabilityService(new StubCapabilityServiceClient(capabilities: expected))
                .Build();

            var result = await sut.GetAll();

            Assert.Equal(
                expected: expected,
                actual: result.Value.Items
            );
        }

        [Fact]
        public async Task returns_expected_when_creating_new_capability()
        {
            var expected = new CapabilityListItemBuilder().Build();

            var sut = new CapabilityApiControllerBuilder()
                .WithCapabilityService(new StubCapabilityServiceClient(capabilities: expected))
                .Build();

            var dummyInput = new CapabilityInput();

            var result = (CreatedAtRouteResult) await sut.CreateCapability(dummyInput);

            Assert.Equal(
                expected: expected,
                actual: result.Value
            );
        }

        [Fact]
        public async Task returns_badrequest_when_creating_new_capability_with_invalid_name()
        {
            var expected = new CapabilityListItemBuilder().Build();

            var sut = new CapabilityApiControllerBuilder()
                .WithCapabilityService(new ErroneousCapabilityServiceClient(new CapabilityValidationException("booo")))
                .Build();

            var dummyInput = new CapabilityInput();

            var result = await sut.CreateCapability(dummyInput);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task returns_expected_when_single_capability_found_by_id()
        {
            var expected = new CapabilityListItemBuilder().Build();

            var sut = new CapabilityApiControllerBuilder()
                .WithCapabilityService(new StubCapabilityServiceClient(capabilities: expected))
                .Build();

            var result = await sut.GetById(expected.Id);

            Assert.Equal(
                expected: expected,
                actual: result.Value
            );
        }

        [Fact]
        public async Task returns_expected_when_user_joins_a_capability_and_user_has_already_joined()
        {
            var sut = new CapabilityApiControllerBuilder()
                .WithCapabilityService(new ErroneousCapabilityServiceClient(new AlreadyJoinedException()))
                .Build();

            var result = await sut.JoinCapability("foo", new JoinCapabilityInput {Email = "bar"});

            Assert.Null(result.Value);
        }
    }
}