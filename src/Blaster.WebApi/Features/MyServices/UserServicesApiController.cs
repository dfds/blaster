using System.Threading.Tasks;
using DFDS.TeamService.WebApi.Features.UserServices.model;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.MyServices
{
    [ApiController]
    public class UserServicesApiController : ControllerBase
    {
        private readonly IUserServicesService _userServicesService;
        
        public UserServicesApiController(IUserServicesService userServicesService)
        {
            _userServicesService = userServicesService;
        }
        
        [HttpGet("api/users/{userId}/services")]
        public async Task<TeamsDTO> GetServices(string userId)
        {


            return await _userServicesService.GetServices(userId);
        }
    }
}