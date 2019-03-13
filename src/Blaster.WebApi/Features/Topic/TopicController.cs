using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Topic
{
    [Route("topics")]
    public class TopicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}