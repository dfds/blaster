using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Cognito.WebApi.Failures;

namespace Cognito.WebApi.Model
{
    public class UserPoolClient
    {
        private readonly AmazonCognitoIdentityProviderClient _identityProviderClient;
        private readonly string _userPoolId;


        public UserPoolClient(
            string accessKey,
            string secretKey,
            string userPoolId
        )
        {
            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            _identityProviderClient =
                new AmazonCognitoIdentityProviderClient(awsCredentials, RegionEndpoint.EUCentral1);


            _userPoolId = userPoolId;
        }

        public async Task<Result<Nothing, NotFound>> AddUserToGroup(
            string groupName,
            string username
        )
        {
            var userToGroupRequest = new AdminAddUserToGroupRequest
            {
                Username = username,
                GroupName = groupName,
                UserPoolId = _userPoolId
            };

            try
            {
                await _identityProviderClient.AdminAddUserToGroupAsync(userToGroupRequest);
            }
            catch (ResourceNotFoundException exception) when (exception.Message == "Group not found.")
            {
                var message = $"the group '{groupName}' does not exist";
                return new Result<Nothing, NotFound>(new NotFound(message));
            }
            catch (UserNotFoundException exception) when (exception.Message == "User does not exist.")
            {
                var message = $"the user '{username}' does not exist";
                return new Result<Nothing, NotFound>(new NotFound(message));
            }


            return new Result<Nothing, NotFound>(new Nothing());
        }

        
        public async Task<Result<List<AttributeType>, NotFound>> GetUserAttributes(string userName)
        {
            var getUserRequest = new AdminGetUserRequest
            {
                Username = userName,
                UserPoolId = _userPoolId
            };
            try
            {
                var getUserResponse = await _identityProviderClient.AdminGetUserAsync(getUserRequest);
                return new Result<List<AttributeType>, NotFound>(getUserResponse.UserAttributes);
            }
            catch (UserNotFoundException exception) when (exception.Message == "User does not exist.")
            {
                var message = $"the user '{userName}' does not exist";
                return new Result<List<AttributeType>, NotFound>(new NotFound(message));
            }
        }


        public async Task CreateUserAsync(string userName)
        {
            var createUserRequest = new AdminCreateUserRequest
            {
                UserPoolId = _userPoolId,
                Username = userName,
                MessageAction = MessageActionType.SUPPRESS
                //    UserAttributes = new List<AttributeType> {new AttributeType {Name = "shoe-color", Value = "brown"}}
            };

            await _identityProviderClient.AdminCreateUserAsync(createUserRequest);
        }

        public async Task CreateGroupAsync(string groupName)
        {
            var createGroupRequest = new CreateGroupRequest();
            createGroupRequest.UserPoolId = _userPoolId;
            createGroupRequest.GroupName = groupName;
            await _identityProviderClient.CreateGroupAsync(createGroupRequest);
        }


        public async Task<GroupType> GetGroupAsync(string groupName)
        {
            var getGroupRequest = new GetGroupRequest
            {
                GroupName = groupName,
                UserPoolId = _userPoolId
            };

            try
            {
                var getGroupResponse = await _identityProviderClient.GetGroupAsync(getGroupRequest);

                return getGroupResponse.Group;
            }
            catch (ResourceNotFoundException)
            {
                return null;
            }
        }


        public async Task<List<UserType>> ListUsersInGroupAsync(string groupName)
        {
            string nextToken = null;
            var users = new List<UserType>();

            do
            {
                var listUsersInGroupRequest = new ListUsersInGroupRequest
                {
                    GroupName = groupName,
                    UserPoolId = _userPoolId,
                    NextToken = nextToken
                };

                var usersInGroupResponse = await _identityProviderClient.ListUsersInGroupAsync(listUsersInGroupRequest);
                nextToken = usersInGroupResponse.NextToken;

                users.AddRange(usersInGroupResponse.Users);
            } while (nextToken != null);


            return users;
        }


        public async Task<IEnumerable<string>> ListGroupsAsync()
        {
            var groupNames = new List<string>();
            string nextToken = null;
            do
            {
                var listGroupsRequest = new ListGroupsRequest
                {
                    UserPoolId = _userPoolId,
                    NextToken = nextToken
                };

                var listGroupsResponse = await _identityProviderClient.ListGroupsAsync(listGroupsRequest);
                nextToken = listGroupsResponse.NextToken;

                groupNames.AddRange(listGroupsResponse.Groups.Select(g => g.GroupName));
            } while (nextToken != null);


            return groupNames;
        }
    }
}