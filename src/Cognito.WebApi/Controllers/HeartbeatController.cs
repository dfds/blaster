using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("system")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        
        public ActionResult<string> Get()
        {
            return "Cognito WebApi says this is fine";
        }
    }
}