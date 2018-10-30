using System.Threading.Tasks;
using DFDS.TeamService.WebApi.Features.UserServices.model;

namespace Blaster.WebApi.Features.MyServices
{
    public interface IUserServicesService
    {
        Task<TeamsDTO> GetServices(string userId);
    }
}