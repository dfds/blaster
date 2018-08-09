using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Dashboards
{
    [Route("dashboards")]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var vm = new DashboardListViewModel
            {
                //Items = new[]
                //{
                //    new DashboardListItem
                //    {
                //        Id = "DA83FD49-EA81-4DC6-8DA4-B4E00BD7535F",
                //        Name = "Blaster - RED",
                //        Team = "DED"
                //    },
                //    new DashboardListItem
                //    {
                //        Id = "DA83FD49-EA81-4DC6-8DA4-B4E00BD7535F",
                //        Name = "Blaster - RED",
                //        Team = "DED"
                //    },
                //    new DashboardListItem
                //    {
                //        Id = "DA83FD49-EA81-4DC6-8DA4-B4E00BD7535F",
                //        Name = "Blaster - RED",
                //        Team = "DED"
                //    },
                //}
            };

            return View(vm);
        }
    }

    public class DashboardListViewModel
    {

    }
}