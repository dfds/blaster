using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilities
{
    [Route("capability")]
    public class CapabilityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}