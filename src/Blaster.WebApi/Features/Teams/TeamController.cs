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

    [Route("api/teams")]
    [ApiController]
    public class TeamApiController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;

        public TeamApiController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }

        [Route("")]
        [HttpGet(Name = "GetAllTeams")]
        public async Task<ActionResult<TeamListResponse>> GetAll()
        {
            var teams = await _cognitoService.GetAll();

            return teams ?? new TeamListResponse
            {
                Items = new TeamListItem[0]
            };
        }
    }

    public class TeamListResponse
    {
        public TeamListItem[] Items { get; set; }
    }

    public class TeamListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }

    [Route("api/teams")]
    [ApiController]
    public class TeamApiController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;

        public TeamApiController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }

        [Route("")]
        [HttpGet(Name = "GetAllTeams")]
        public async Task<ActionResult<TeamListResponse>> GetAll()
        {
            var teams = await _cognitoService.GetAll();

            return teams ?? new TeamListResponse
            {
                Items = new TeamListItem[0]
            };
        }
    }

    public class TeamListResponse
    {
        public TeamListItem[] Items { get; set; }
    }

    public class TeamListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }
}