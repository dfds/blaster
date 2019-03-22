using System.Threading.Tasks;
using Blaster.WebApi.Features.AWSPermissions.Models;

namespace DefaultNamespace
{
    public interface IAWSJanitorClient
    {
        Task<PolicyDTO[]> GetPoliciesByCapabilityNameAsync(string capabilityName);
    }
}