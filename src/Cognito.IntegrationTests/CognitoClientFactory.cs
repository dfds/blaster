using System;
using System.Threading.Tasks;
using Cognito.WebApi;

namespace Cognito.IntegrationTests
{
    public class CognitoClientFactory
    {
        public static CognitoClient CreateFromGetEnvironmentVariables()
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_accessKey");
            var secretKey = Environment.GetEnvironmentVariable("AWS_secretKey");

            var client = new CognitoClient(
                accessKey,
                secretKey
            );


            return client;
        }
    }
}