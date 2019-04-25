using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.CapabilityDetails
{
    [Route("capabilitydetails")]
    public class CapabilityDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}