using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.System
{
    [Route("system")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;

        public HealthController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }

        [Route("health")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var greeting = await _cognitoService.SayHello();
            return greeting;
        }
    }
 }
