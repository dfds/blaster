using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.WebApi.Features.Teams
{
    public interface ITeamServiceClient
    {
        Task<TeamsResponse> GetAll();
        Task<Team> CreateTeam(string name);
        Task<Team> GetById(string id);
        Task JoinTeam(string teamId, string memberEmail);
    }
}