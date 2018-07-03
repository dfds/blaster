using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Controllers
{
    [Route("")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        public ActionResult Get()
        {
            return Ok("hello world");
        }
    }
}
