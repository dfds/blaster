using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Cognito.WebApi;

namespace Cognito.IntegrationTests
{
    public class CognitoClientFacts
    {
        [Fact]
        public async Task CreateGroup()
        {
            var client = CreateClient();


            try
            {
                var groupName = CreateName();
                await client.CreateGroupAsync(groupName);
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }

        [Fact]
        public async Task CreateUser()
        {
            var client = CreateClient();

            try
            {
                var userName = CreateName();
                await client.CreateUser(userName);
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }

        [Fact]
        public async Task AddUserToGroup()
        {
            var client = CreateClient();

            try
            {
                var userName = CreateName();
                await client.CreateUser(userName);

                var groupName = CreateName();
                await client.CreateGroupAsync(groupName);

                await client.AddUserToGroup(userName, groupName);
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
            var client = CreateClient();

            try
            {
                var groupName = CreateName();
                await client.CreateGroupAsync(groupName);

                // Act
                var groupsResult = await client.ListGroupsAsync();

                // Assert
                Assert.Equal(groupName, groupsResult.Single());
            }
            finally
            {
                await client.DeleteUserPoolAsync();
            }
        }


        [Fact]
        public async Task ListSixtyOneGroups()
        {
            // Arrange
            var client = CreateClient();

            try
            {
                var groups = new List<string>();
                for (var i = 0; i < 62; i++)
                {
                    var groupName = CreateName();
                    await client.CreateGroupAsync(groupName);

                    groups.Add(groupName);
                }

                // Act
                var groupsResult = await client.ListGroupsAsync();

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
            var client = CreateClient();

            try
            {
                var groupName = CreateName();
                var group = await client.GetGroupAsync(groupName);

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


        public CognitoClient CreateClient()
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_accessKey");
            var secretKey = Environment.GetEnvironmentVariable("AWS_secretKey");
            var client = new CognitoClient(
                accessKey,
                secretKey,
                CreateName()
            );


            return client;
        }
    }
}