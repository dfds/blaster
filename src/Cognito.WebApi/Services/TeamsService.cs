using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognito.WebApi.Model;

namespace Cognito.WebApi.Services
{
    public class TeamsService
    {
        private readonly UserPoolClient _userPoolClient;

        public TeamsService(UserPoolClient userPoolClient)
        {
            _userPoolClient = userPoolClient;
        }

        public async Task<List<Team>> GetAllTeams()
        {
            var groupNames = await _userPoolClient.ListGroupsAsync();

            var getTeamsTask = groupNames
                .Select(g => GetTeam(g));


            var teams = (await Task.WhenAll(getTeamsTask)).ToList();
           
            
            return teams;
        }

        public async Task<Team> GetTeam(
            string teamId,
            string departmentId
        )
        {
            var groupName = $"{teamId}_D_{departmentId}";

            return await GetTeam(groupName);
        }

        private async Task<Team> GetTeam(string groupName)
        {
            var group = await _userPoolClient.GetGroupAsync(groupName);
            var usersInGroup = await _userPoolClient.ListUsersInGroupAsync(groupName);

            var team = new Team
            {
                Name = group.GroupName,
                Members = usersInGroup
                    .Select(u =>
                        new User {Id = u.Username}
                    ).ToList()
            };


            return team;
        }


        public async Task CreateTeam(CreateTeam createTeam)
        {
            var groupName = $"{createTeam.Name}_D_{createTeam.DepartmentName}";

            await _userPoolClient.CreateGroupAsync(groupName);
        }
    }
}