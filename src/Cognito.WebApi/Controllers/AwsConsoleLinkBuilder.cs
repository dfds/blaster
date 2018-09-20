using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cognito.WebApi.Controllers
{
    public class AwsConsoleLinkBuilder : IAwsConsoleLinkBuilder
    {
        public async Task<Uri> GenerateUriForConsole(string idToken)
        {
          var cred = new CognitoAWSCredentials("eu-central-1:07c2b8e5-42a1-4791-a006-ea4e14611de8", RegionEndpoint.EUCentral1);
            cred.AddLogin("cognito-idp.eu-central-1.amazonaws.com/eu-central-1_y2kIM3LEL", idToken);

            var client = new AmazonSecurityTokenServiceClient(cred);
            var assumedRole = await client.AssumeRoleAsync(new AssumeRoleRequest
            {
                //RoleArn = "arn:aws:iam::515815634748:role/Cognito_FutteIdentityAuth1_Role",
                RoleArn = "arn:aws:iam::528563840976:role/Cognito_BlasterAuth_Role",
                RoleSessionName = "AssumeRoleSession"
            });

            var payload = new CredentialsPayload
            {
                SessionId = assumedRole.Credentials.AccessKeyId,
                SessionKey = assumedRole.Credentials.SecretAccessKey,
                SessionToken = assumedRole.Credentials.SessionToken
            };


            var sb = new StringBuilder();
            sb.Append("?Action=getSigninToken");
            //sb.Append("&SessionDuration=43200");
            sb.Append("&Session=");

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };


            var jsonPart = WebUtility.UrlEncode(JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            }));

            sb.Append(jsonPart);

            var httpClient = new HttpClient();
            var uri = new Uri("https://signin.aws.amazon.com/federation" + sb);
            var result = await httpClient.GetAsync(uri);

            if (!result.IsSuccessStatusCode)
                throw new ApplicationException("Failed to create signintoken");
                    
            var content = await result.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(content);

            sb.Clear();
            sb.Append("?Action=login");
            sb.Append("&Issuer=app.dfds.cloud");
            sb.Append("&Destination=" + WebUtility.UrlEncode("https://console.aws.amazon.com/"));
            sb.Append("&SigninToken=" + token.SigninToken);
            uri = new Uri("https://signin.aws.amazon.com/federation" + sb);
            return uri;
        }
    }

    public class CredentialsPayload
    {
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
        public string SessionToken { get; set; }
    }

    public class Token
    {
        public string SigninToken { get; set; }
    }
}