using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Cognito.WebApi.Failures;
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


        private async Task<Team> GetTeam(string id)
        {
            var usersInGroup = await _userPoolClient.ListUsersInGroupAsync(id);

            var teamNameAndDepartment = id.Split(new[] {"_D_"}, StringSplitOptions.None);
            var departmentName = 1 < teamNameAndDepartment.Length ? teamNameAndDepartment[1] : null;

            var team = new Team
            {
                Id = id,
                Name = teamNameAndDepartment[0],
                Department = departmentName,
                Members = usersInGroup
                    .Select(u =>
                        new User {Id = u.Username}
                    ).ToList()
            };


            return team;
        }


        public async Task<Result<Team, IFailure>> CreateTeam(CreateTeam createTeam)
        {
            if (string.IsNullOrWhiteSpace(createTeam.Name))
            {
                var validationFailed = new ValidationFailed();
                validationFailed.Add(nameof(createTeam.Name), "can not be empty");
                return new Result<Team, IFailure>(validationFailed);
            }

            var groupName = $"{createTeam.Name}_D_{createTeam.DepartmentName}";


            var existingTeam = await _userPoolClient.GetGroupAsync(groupName);
            if (existingTeam != null)
            {
                return new Result<Team, IFailure>(
                    new Conflict($"a team with the name {createTeam.Name} already exists"));
            }

            await _userPoolClient.CreateGroupAsync(groupName);

            var team = new Team
            {
                Id = groupName,
                Name = createTeam.Name,
                Department = createTeam.DepartmentName
            };


            return new Result<Team, IFailure>(team);
        }


        public async Task<Result<Nothing, NotFound>> JoinTeam(string teamId, string userId)
        {
           return await _userPoolClient.AddUserToGroup(teamId, userId);
        }
    }
}