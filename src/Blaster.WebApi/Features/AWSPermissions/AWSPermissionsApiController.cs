using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.AWSPermissions
{
    [Route("api/topics")]
    [ApiController]
    public class AWSPermissionsApiController : ControllerBase
    {
        [HttpGet("", Name = "GetAllAWSPermissions")]
        public async Task<ActionResult<AWSPermissionsListResponse>> GetAll()
        {
            
        }
    }
}