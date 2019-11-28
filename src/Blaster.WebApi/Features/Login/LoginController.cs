using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Login
{
	[Route("login")]
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
