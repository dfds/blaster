using System.Threading.Tasks;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.System.Models;
using Blaster.WebApi.Features.Teams;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.MyServices
{
    [Route("myservices")]
    public class MyServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}