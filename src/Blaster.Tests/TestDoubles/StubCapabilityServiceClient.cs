using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubCapabilityServiceClient : ICapabilityServiceClient
    {
        private readonly Member _member;
        private readonly Capability[] _capabilities;

        public StubCapabilityServiceClient(Member member = null, params Capability[] capabilities)
        {
            _member = member;
            _capabilities = capabilities;
        }

        public Task<CapabilitiesResponse> GetAll()
        {
            return Task.FromResult(new CapabilitiesResponse
            {
                Items = _capabilities,
            });
        }

        public Task<Capability> CreateCapability(string name)
        {
            return Task.FromResult(_capabilities.First());
        }

        public Task<Capability> GetById(string id)
        {
            return Task.FromResult(_capabilities.FirstOrDefault());
        }

        public Task JoinCapability(string teamId, string memberEmail)
        {
            return Task.CompletedTask;
        }

        public Task LeaveCapability(string teamId, string memberEmail)
        {
            return Task.CompletedTask;
        }
    }
}