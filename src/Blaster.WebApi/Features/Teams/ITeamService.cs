using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.WebApi.Features.Teams
{
    public interface ITeamService
    {
        Task<TeamListResponse> GetAll();
        Task<TeamListItem> CreateTeam(string name, string department);
        Task<TeamListItem> GetById(string id);
        Task<Member> JoinTeam(string teamId, string userId);
    }
}