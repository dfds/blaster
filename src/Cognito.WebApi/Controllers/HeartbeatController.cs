using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.WebApi.Controllers
{
    [Route("system")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly CognitoClient _cognitoClient;

        public HealthController(CognitoClient cognitoClient)
        {
            _cognitoClient = cognitoClient;
        }

        [HttpGet("health")]
        public async Task<ActionResult<string>> Get(bool deep)
        {
            const string allISWell="Cognito WebApi says this is fine";
            if (deep == false)
            {
                return allISWell;
            }

            var cognitoIsAlive = await _cognitoClient.IsAlive();

            return cognitoIsAlive ? Ok(allISWell): StatusCode(504, "No connection to AWS cognito can be made");
        }
    }
}