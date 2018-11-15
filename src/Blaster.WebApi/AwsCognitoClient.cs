using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;

namespace Blaster.WebApi
{
    public class AwsCognitoClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _poolId;
        private readonly RegionEndpoint _regionEndpoint;

        public AwsCognitoClient(
            string clientId,
            string clientSecret,
            string poolId,
            string regionEndpoint
        )
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _poolId = poolId;
            _regionEndpoint = RegionEndpoint.GetBySystemName(regionEndpoint);
        }


        public async Task<AuthenticationResultType> RefreshToken(
            string userName,
            string refreshToken
        )
        {
            var cognitoIdentityProviderClient =
                new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), _regionEndpoint);

            var userPool = new CognitoUserPool(_poolId, _clientId, cognitoIdentityProviderClient);

            var user = new CognitoUser(
                userName,
                _clientId,
                userPool,
                cognitoIdentityProviderClient,
                _clientSecret
            );


            user.SessionTokens =
                new CognitoUserSession(
                    idToken: null,
                    accessToken: null,
                    refreshToken: refreshToken,
                    issuedTime: DateTime.Now,
                    expirationTime: DateTime.Now.AddDays(3)
                );

            var context = await user.StartWithRefreshTokenAuthAsync(
                new InitiateRefreshTokenAuthRequest {AuthFlowType = AuthFlowType.REFRESH_TOKEN}
            ).ConfigureAwait(false);


            return context.AuthenticationResult;
        }
    }
}