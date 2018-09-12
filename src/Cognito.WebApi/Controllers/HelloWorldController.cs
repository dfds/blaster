using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult<string> Get(int id)
        {
            return "hello world";
        }
    }
}