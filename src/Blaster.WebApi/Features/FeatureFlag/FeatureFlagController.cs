using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.FeatureFlag
{
    [Route("featureflags")]
    public class FeatureFlagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}