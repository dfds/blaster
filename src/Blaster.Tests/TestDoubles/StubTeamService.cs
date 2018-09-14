using System.Linq;
using System.Threading.Tasks;
using Blaster.Tests.Features.Teams;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Features.Teams.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubTeamService : ITeamService
    {
        private readonly TeamListItem[] _teams;

        public StubTeamService(params TeamListItem[] teams)
        {
            _teams = teams;
        }

        public Task<TeamListResponse> GetAll()
        {
            return Task.FromResult(new TeamListResponse
            {
                Items = _teams,
            });
        }

        public Task<TeamListItem> CreateTeam(string name, string department)
        {
            return Task.FromResult(_teams.First());
        }

        public Task<TeamListItem> GetById(string id)
        {
            return Task.FromResult(_teams.FirstOrDefault());
        }
    }
}