using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.ContainerRegistry
{
    [Route("containerregistry")]
    public class ContainerRegistryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}