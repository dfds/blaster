using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.AWS
{
    [Route("aws")]
    public class AwsConsoleUrlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}