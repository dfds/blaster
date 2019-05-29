using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Topic
{
    [Route("topicdetails")]
    public class TopicDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}