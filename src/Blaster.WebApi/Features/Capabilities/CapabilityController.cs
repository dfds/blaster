using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Teams
{
    [Route("teams")]
    public class CapabilityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}