using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilitiesv2
{
    [Route("capabilitiesv2")]
    public class Capabilityv2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}