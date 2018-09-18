using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;

namespace Cognito.WebApi
{
    public class CognitoClient
    {
        private readonly AmazonCognitoIdentityProviderClient _identityProviderClient;

        public CognitoClient(
            string accessKey,
            string secretKey
        )
        {
            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            _identityProviderClient =
                new AmazonCognitoIdentityProviderClient(awsCredentials, RegionEndpoint.EUCentral1);
        }


        public async Task DeleteUserPoolAsync(string userPoolId)
        {
            var deleteUserPoolRequest = new DeleteUserPoolRequest
            {
                UserPoolId = userPoolId
            };
            await _identityProviderClient.DeleteUserPoolAsync(deleteUserPoolRequest);
        }


        public async Task<string> CreateUserPoolAsync(string poolName)
        {
            var createUserPoolRequest = new CreateUserPoolRequest
            {
                PoolName = poolName
            };

            var response = await _identityProviderClient.CreateUserPoolAsync(createUserPoolRequest);


            return response.UserPool.Id;
        }


 

      
    }
}