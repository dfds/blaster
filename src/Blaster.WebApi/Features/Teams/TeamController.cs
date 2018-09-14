using System.Threading.Tasks;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Teams
{
    [Route("teams")]
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}