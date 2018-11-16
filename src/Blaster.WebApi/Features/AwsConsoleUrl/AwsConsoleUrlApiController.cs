using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.AWS
{
    [ApiController]
    public class AwsConsoleUrlApiController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;

        public AwsConsoleUrlApiController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }

        [HttpGet("api/teams/{id}/aws/console-url")]
        public async Task<ActionResult<AwsConsoleLinkResponse>> GetConsoleUrl(Guid id)
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var link = await _cognitoService.GetAwsConsoleLink(id, idToken);

            return link ?? new AwsConsoleLinkResponse()
            {
                AbsoluteUrl = ""
            };
        }
    }
}