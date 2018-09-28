namespace Cognito.WebApi
{
    public interface IVariables
    {
        string AwsCognitoAccessKey { get; }
        string AwsCognitoSecretAccessKey { get; }
        string AwsCognitoUserPoolId { get; }
        
        string AwsCognitoLoginProviderName { get; }
        string AwsCognitoIdentityPoolId { get; }
        
        void Validate();
    }
}