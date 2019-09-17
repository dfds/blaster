using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Frontpage
{
    [Route("")]
    public class FrontpageController : Controller
    {
        private readonly IIamRoleService _client;

        public FrontpageController(IIamRoleService client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/sharedcomponents")]
        public IActionResult SharedComponents()
        {
            return View();
        }

        [HttpGet("/downloads/kubeconfig")]
        public async Task<IActionResult> DownloadKubeConfig()
        {
            var defaultKubeConfig = await _client.GetDefaultKubeConfig();

            return File(defaultKubeConfig.Content, defaultKubeConfig.ContentType, defaultKubeConfig.FileName);
        }
    }
}