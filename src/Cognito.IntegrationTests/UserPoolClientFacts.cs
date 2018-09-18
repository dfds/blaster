using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Cognito.WebApi;
using Cognito.WebApi.Model;

namespace Cognito.IntegrationTests
{
    public class CognitoClientFacts
    {
        [Fact]
        public async Task CreateGroup()
        {
            var client = await CreateClient();


            try
            {
                var userPoolClient = CreateUserPoolClient(client.UserPoolId);

                var groupName = CreateName();
                await userPoolClient.CreateGroupAsync(groupName);
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }

        [Fact]
        public async Task CreateUser()
        {
            var client = await CreateClient();

            try
            {
                var userPoolClient = CreateUserPoolClient(client.UserPoolId);

                var userName = CreateName();
                await userPoolClient.CreateUser(userName);
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }

        [Fact]
        public async Task AddUserToGroup()
        {
            var client = await CreateClient();

            try
            {
                var userPoolClient = CreateUserPoolClient(client.UserPoolId);

                var userName = CreateName();
                await userPoolClient.CreateUser(userName);

                var groupName = CreateName();
                await userPoolClient.CreateGroupAsync(groupName);

                await userPoolClient.AddUserToGroup(userName, groupName);
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }


        [Fact]
        public async Task ListOneGroup()
        {
            // Arrange
            var client = await CreateClient();

            try
            {
                var userPoolClient = CreateUserPoolClient(client.UserPoolId);

                var groupName = CreateName();
                await userPoolClient.CreateGroupAsync(groupName);

                // Act
                var groupsResult = await userPoolClient.ListGroupsAsync();

                // Assert
                Assert.Equal(groupName, groupsResult.Single());
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }


        [Fact]
        public async Task ListTwentyFiveGroups()
        {
            // Arrange
            var client = await CreateClient();

            try
            {
                var userPoolClient = CreateUserPoolClient(client.UserPoolId);

                var groups = new List<string>();
                for (var i = 0; i < 25; i++)
                {
                    var groupName = CreateName();
                    await userPoolClient.CreateGroupAsync(groupName);

                    groups.Add(groupName);
                }

                // Act
                var groupsResult = await userPoolClient.ListGroupsAsync();

                // Assert
                groupsResult.ShouldBe(groups);
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }


        [Fact]
        public async Task GetAGroupThatDoesNotExist()
        {
            // Arrange
            var client = await CreateClient();

            try
            {
                var userPoolClient = CreateUserPoolClient(client.UserPoolId);
                var groupName = CreateName();
                var group = await userPoolClient.GetGroupAsync(groupName);

                group.ShouldBeNull();
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }


        public string CreateName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-THH-mm") + "_" + Guid.NewGuid().ToString().Substring(0, 8);
        }

        public UserPoolClient CreateUserPoolClient(string userPoolId)
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_accessKey");
            var secretKey = Environment.GetEnvironmentVariable("AWS_secretKey");

            var userPoolClient = new UserPoolClient(
                accessKey,
                secretKey,
                userPoolId
            );


            return userPoolClient;
        }

        
        
        public async Task<CognitoClient> CreateClient()
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_accessKey");
            var secretKey = Environment.GetEnvironmentVariable("AWS_secretKey");

            var client = new CognitoClient(
                accessKey,
                secretKey,
                null
            );

            var userPollId = await client.CreateUserPoolAsync(CreateName());


            client = new CognitoClient(
                accessKey,
                secretKey,
                userPollId
            );
            
            
            return client;
        }
    }
}