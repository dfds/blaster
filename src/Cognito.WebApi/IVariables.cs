namespace Cognito.WebApi
{
    public interface IVariables
    {
        string AwsCognitoAccessKey { get; }
        string AwsCognitoSecretAccessKey { get; }
        string AwsCognitoIdentityPoolIdKey { get; }
        string AwsCognitoUserPoolProviderKey { get; }
        string AwsCognitoUserPoolId { get; }
        void Validate();
    }
}