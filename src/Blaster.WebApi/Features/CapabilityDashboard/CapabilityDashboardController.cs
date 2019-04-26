using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.CapabilityDashboard
{
    [Route("capabilitydashboard")]
    public class CapabilityDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}