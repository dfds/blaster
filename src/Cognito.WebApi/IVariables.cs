namespace Cognito.WebApi
{
    public interface IVariables
    {
        string AwsCognitoAccessAccessKey { get; }
        string AwsCognitoSecretAccessKey { get; }
        string AwsCognitoIdentityPoolIdKey { get; }
        string AwsCognitoUserPoolProviderKey { get; }
        void Validate();
    }
}