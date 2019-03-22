using System.Threading.Tasks;
using Blaster.WebApi.Features.AWSPermissions.Models;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blaster.WebApi.Features.AWSPermissions
{
    [Route("api/awspermissions")]
    [ApiController]
    public class AWSPermissionsApiController : ControllerBase
    {
        private readonly IAWSJanitorClient _awsJanitorClient;

        public AWSPermissionsApiController(IAWSJanitorClient awsJanitorClient)
        {
            _awsJanitorClient = awsJanitorClient;
        }

        [HttpGet("{key}", Name = "GetAllAWSPermissionsByKey")]
        public async Task<ActionResult<AWSPermissionsListResponse>> GetAllAWSPermissionsByKey(string key)
        {
            if (key == "Fo")
            {
                return BadRequest();
            }
            var list = new AWSPermissionsListResponse
            {
                Items = await _awsJanitorClient.GetPoliciesByCapabilityNameAsync(key)
            };

            return list;
        }
    }
}