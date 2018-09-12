using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.System
{
    [Route("system")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [Route("health")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "";
        }
    }
}