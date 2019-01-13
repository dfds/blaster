using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubTeamService : ITeamService
    {
        private readonly Member _member;
        private readonly TeamListItem[] _teams;

        public StubTeamService(Member member = null, params TeamListItem[] teams)
        {
            _member = member;
            _teams = teams;
        }

        public Task<TeamListResponse> GetAll()
        {
            return Task.FromResult(new TeamListResponse
            {
                Items = _teams,
            });
        }

        public Task<TeamListItem> CreateTeam(string name)
        {
            return Task.FromResult(_teams.First());
        }

        public Task<TeamListItem> GetById(string id)
        {
            return Task.FromResult(_teams.FirstOrDefault());
        }

        public Task<Member> JoinTeam(string teamId, string userId)
        {
            return Task.FromResult(_member);
        }
    }
}