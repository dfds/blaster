using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubTeamService : ITeamService
    {
        private readonly Member _member;
        private readonly Team[] _teams;

        public StubTeamService(Member member = null, params Team[] teams)
        {
            _member = member;
            _teams = teams;
        }

        public Task<TeamsResponse> GetAll()
        {
            return Task.FromResult(new TeamsResponse
            {
                Items = _teams,
            });
        }

        public Task<Team> CreateTeam(string name)
        {
            return Task.FromResult(_teams.First());
        }

        public Task<Team> GetById(string id)
        {
            return Task.FromResult(_teams.FirstOrDefault());
        }

        public Task JoinTeam(string teamId, string memberEmail)
        {
            return Task.CompletedTask;
        }
    }
}