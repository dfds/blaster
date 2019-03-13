using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.Tests.TestDoubles
{
    public class ErroneousCapabilityServiceClient : ICapabilityServiceClient
    {
        private readonly Exception _error;

        public ErroneousCapabilityServiceClient(Exception error)
        {
            _error = error;
        }

        public Task<CapabilitiesResponse> GetAll()
        {
            throw _error;
        }

        public Task<Capability> CreateCapability(string name)
        {
            throw _error;
        }

        public Task<Capability> GetById(string id)
        {
            throw _error;
        }

        public Task JoinCapability(string capabilityId, string memberEmail)
        {
            throw _error;
        }

        public Task LeaveCapability(string capabilityId, string memberEmail)
        {
            throw _error;
        }
    }
}