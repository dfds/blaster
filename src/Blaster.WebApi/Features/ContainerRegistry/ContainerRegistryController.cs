using System.Threading.Tasks;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams.Models;
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