using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Blaster.WebApi.Features.Dashboards
{
    [Route("api/dashboards")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardApiController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("", Name = "GetAll")]
        public async Task<ActionResult<DashboardListResponse>> GetAll()
        {
            var dashboards = await _dashboardService.GetAll();

            return dashboards ?? new DashboardListResponse
            {
                Items = new DashboardListItem[0],
                TotalCount = 0
            };
        }

        [HttpGet("{id}", Name = "GetSingleDashboard")]
        public async Task<ActionResult<DashboardDetailItem>> GetById(string id)
        {
            var dashboard = await _dashboardService.Get(id);

            if (dashboard != null)
            {
                return new ActionResult<DashboardDetailItem>(dashboard);
            }

            return new ActionResult<DashboardDetailItem>(NotFound());
        }

        [HttpPost("", Name = "CreateSingleDashboard")]
        public async Task<CreatedAtRouteResult<DashboardDetailItem>> Post([FromBody] DashboardInput input)
        {
            var dashboard = await _dashboardService.Create(input);

            return new CreatedAtRouteResult<DashboardDetailItem>(
                routeName: "GetSingleDashboard",
                routeValues: new {id = dashboard.Id},
                value: dashboard
            );
        }
    }

    public class CreatedAtRouteResult<T> : IConvertToActionResult
    {
        private readonly string _routeName;
        private readonly object _routeValues;

        public CreatedAtRouteResult(string routeName, object routeValues, T value)
        {
            _routeName = routeName;
            _routeValues = routeValues;
            Value = value;
        }

        public T Value { get; }

        public IActionResult Convert()
        {
            return new CreatedAtRouteResult(_routeName, _routeValues, Value);
        }
    }
}