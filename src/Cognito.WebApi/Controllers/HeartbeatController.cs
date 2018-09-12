using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            return "Cognito WebApi says hello";
        }
    }
}