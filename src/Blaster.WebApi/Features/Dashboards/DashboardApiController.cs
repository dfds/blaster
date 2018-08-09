using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Dashboards
{
    [Route("api/dashboards")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardApiController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [Route("")]
        public async Task<ActionResult<DashboardListResponse>> Get()
        {
            var dashboards = await _dashboardRepository.GetAll();
            var items = dashboards.ToArray();

            return new DashboardListResponse
            {
                Items = items,
                TotalCount = items.Length,
            };
        }

        [Route("{id}")]
        public async Task<ActionResult<DashboardDetailItem>> Get(string id)
        {
            var item = await _dashboardRepository.Get(id);

            if (item != null)
            {
                return new ActionResult<DashboardDetailItem>(item);
            }

            return new ActionResult<DashboardDetailItem>(NotFound());
        }
    }
}