using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cognito.WebApi.Controllers
{
    public class AwsConsoleLinkService : IAwsConsoleLinkBuilder
    {
        /// <summary>Adds a login to be used for authenticated requests.</summary>
        /// <param name="providerName">The provider name for the login (i.e. graph.facebook.com)</param>
        /// <param name="token">The token provided by the identity provider.</param>
        public async Task<Uri> GenerateUriForConsole(string identityToken)
        {
            var identityPoolId = "eu-central-1:07c2b8e5-42a1-4791-a006-ea4e14611de8";
            var providerName = "cognito-idp.eu-central-1.amazonaws.com/eu-central-1_y2kIM3LEL";
            
            var credentialsPayload = await AssumeRole(
                identityPoolId,
                providerName,
                identityToken
            );

            var signinToken = await GetSignInToken(credentialsPayload);

            var consoleLoginUri = BuildConsoleLoginUri(signinToken);


            return consoleLoginUri;
        }

        
        public async Task<CredentialsPayload> AssumeRole(
            string identityPoolId, 
            string providerName, 
            string identityToken
        )
        {
            var regionEndpoint = RegionEndpoint.GetBySystemName(identityPoolId.Split(':')[0]);
            
            var cognitoAwsCredentials = new CognitoAWSCredentials(
                identityPoolId,
                regionEndpoint
            );
            
            cognitoAwsCredentials.AddLogin(
                providerName, 
                identityToken
            );

            var securityTokenServiceClient = new AmazonSecurityTokenServiceClient(cognitoAwsCredentials);
            var assumedRole = await securityTokenServiceClient.AssumeRoleAsync(new AssumeRoleRequest
                {
                    RoleArn = "arn:aws:iam::528563840976:role/Hest",
                    RoleSessionName = "AssumeRoleSession"
                }
            );

            var credentialsPayload = new CredentialsPayload
            {
                SessionId = assumedRole.Credentials.AccessKeyId,
                SessionKey = assumedRole.Credentials.SecretAccessKey,
                SessionToken = assumedRole.Credentials.SessionToken
            };


            return credentialsPayload;
        }   
        
        
        public Uri BuildConsoleLoginUri(string signinToken)
        {
            var sb = new StringBuilder();
            sb.Append("https://signin.aws.amazon.com/federation");
            sb.Append("?Action=login");
            sb.Append("&Issuer=app.dfds.cloud");
            sb.Append("&Destination=" + WebUtility.UrlEncode("https://console.aws.amazon.com/"));
            sb.Append("&SigninToken=" + signinToken);
            
            var consoleLoginUri = new Uri(sb.ToString());


            return consoleLoginUri;
        }

        
        public async Task<string> GetSignInToken(CredentialsPayload credentialsPayload)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var session = WebUtility.UrlEncode(JsonConvert.SerializeObject(
                credentialsPayload, 
                new JsonSerializerSettings
                {
                    ContractResolver = contractResolver
                }
            ));

            var httpClient = new HttpClient();

            var uri = new Uri($"https://signin.aws.amazon.com/federation?Action=getSigninToken&Session={session}");
            var httpResponse = await httpClient.GetAsync(uri);

            if (!httpResponse.IsSuccessStatusCode)
                throw new ApplicationException("Failed to create signintoken");

            var content = await httpResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(content);


            return token.SigninToken;
        }
    }
}