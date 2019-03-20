using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.AWSPermissions
{
    [Route("awspermissions")]
    public class AWSPermissionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}