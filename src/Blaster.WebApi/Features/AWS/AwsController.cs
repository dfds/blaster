using System.Threading.Tasks;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.AWS
{
    [Route("aws")]
    public class AwsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    [Route("api/aws")]
    [ApiController]
    public class AwsApiController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;

        public AwsApiController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }

        [Route("")]
        [HttpGet(Name = "GetAwsConsoleLink")]
        public async Task<ActionResult<AwsConsoleLinkResponse>> GetAll()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var link = await _cognitoService.GetAwsConsoleLink(idToken);

            return link ?? new AwsConsoleLinkResponse()
            {
                AbsoluteUrl = ""
            };
        }
    }
}