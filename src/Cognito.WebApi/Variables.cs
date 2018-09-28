using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cognito.WebApi
{
    public class Variables : IVariables
    {
        private const string AwsCognitoAccessKeyIdentifier = "AWS_COGNITO_ACCESS_KEY";
        public string AwsCognitoAccessKey { get; }

        private const string AwsCognitoSecretAccessKeyIdentifier = "AWS_COGNITO_SECRET_ACCESS_KEY";
        public string AwsCognitoSecretAccessKey { get; }

        private const string AwsCognitoUserPoolIdIdentifier = "AWS_COGNITO_USER_POOL_ID";
        public string AwsCognitoUserPoolId { get; }

        private const string AwsCognitoLoginProviderNameIdentifier = "AWS_COGNITO_LOGIN_PROVIDER_NAME";
        public string AwsCognitoLoginProviderName { get; }

        private const string AwsCognitoIdentityPoolIdIdentifier = "AWS_COGNITO_IDENTITY_POOL_ID";
        public string AwsCognitoIdentityPoolId { get; }
       
        public Variables()
        {
            AwsCognitoAccessKey = Environment.GetEnvironmentVariable(AwsCognitoAccessKeyIdentifier);
            AwsCognitoSecretAccessKey = Environment.GetEnvironmentVariable(AwsCognitoSecretAccessKeyIdentifier);
            AwsCognitoUserPoolId = Environment.GetEnvironmentVariable(AwsCognitoUserPoolIdIdentifier);

            AwsCognitoLoginProviderName = Environment.GetEnvironmentVariable(AwsCognitoLoginProviderNameIdentifier);
            AwsCognitoIdentityPoolId = Environment.GetEnvironmentVariable(AwsCognitoIdentityPoolIdIdentifier);
        }

        public void Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(AwsCognitoAccessKey)) { errors.Add(createVariableNotSetString(AwsCognitoAccessKeyIdentifier)); }
            if (string.IsNullOrWhiteSpace(AwsCognitoSecretAccessKey)) { errors.Add(createVariableNotSetString(AwsCognitoSecretAccessKeyIdentifier)); }
            if (string.IsNullOrWhiteSpace(AwsCognitoUserPoolId)) { errors.Add(createVariableNotSetString(AwsCognitoUserPoolIdIdentifier)); }

            if (string.IsNullOrWhiteSpace(AwsCognitoLoginProviderName)) { errors.Add(createVariableNotSetString(AwsCognitoLoginProviderNameIdentifier)); }
            if (string.IsNullOrWhiteSpace(AwsCognitoIdentityPoolId)) { errors.Add(createVariableNotSetString(AwsCognitoIdentityPoolIdIdentifier)); }

            if (errors.Any() == false) { return; }

            var errorMessage = Environment.NewLine + string.Join(Environment.NewLine, errors);
            throw new Exception(errorMessage);
        }

        private string createVariableNotSetString(string variableName)
        {
            return $"\tEnvironment variable '{variableName}' not set";
        }
    }
}