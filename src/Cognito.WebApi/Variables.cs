using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cognito.WebApi
{
    public class Variables : IVariables
    {
        private const string AwsCognitoAccessAccessKeyIdentifier = "AWS_COGNITO_ACCESS_KEY";
        public string AwsCognitoAccessAccessKey { get; }

        private const string AwsCognitoSecretAccessKeyIdentifier = "AWS_COGNITO_SECRET_ACCESS_KEY";
        public string AwsCognitoSecretAccessKey { get; }

        private const string AwsCognitoUserPoolIdIdentifier = "AWS_COGNITO_USER_POOL_ID";
        public string AwsCognitoUserPoolId { get; }

        private const string AwsCognitoIdentityPoolIdKeyIdentifier = "AWS_COGNITO_IDENTITY_POOL_I_KEY";
        public string AwsCognitoIdentityPoolIdKey { get; }

        private const string AwsCognitoUserPoolProviderKeyIdentifier = "AWS_COGNITO_USER_POOL_PROVIDER_KEY";
        public string AwsCognitoUserPoolProviderKey { get; }
        
       


        public Variables()
        {
            AwsCognitoAccessAccessKey = Environment.GetEnvironmentVariable(AwsCognitoAccessAccessKeyIdentifier);
            AwsCognitoSecretAccessKey = Environment.GetEnvironmentVariable(AwsCognitoSecretAccessKeyIdentifier);
            AwsCognitoUserPoolId = Environment.GetEnvironmentVariable(AwsCognitoUserPoolIdIdentifier);


            AwsCognitoIdentityPoolIdKey = Environment.GetEnvironmentVariable(AwsCognitoIdentityPoolIdKeyIdentifier);
            AwsCognitoUserPoolProviderKey = Environment.GetEnvironmentVariable(AwsCognitoUserPoolProviderKeyIdentifier);
        }

        public void Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(AwsCognitoAccessAccessKey)) { errors.Add(createVariableNotSetString(AwsCognitoAccessAccessKeyIdentifier)); }
            if (string.IsNullOrWhiteSpace(AwsCognitoSecretAccessKey)) { errors.Add(createVariableNotSetString(AwsCognitoSecretAccessKeyIdentifier)); }
            if (string.IsNullOrWhiteSpace(AwsCognitoUserPoolId)) { errors.Add(createVariableNotSetString(AwsCognitoUserPoolIdIdentifier)); }

            
            if (string.IsNullOrWhiteSpace(AwsCognitoIdentityPoolIdKey)) { errors.Add(createVariableNotSetString(AwsCognitoIdentityPoolIdKeyIdentifier)); }
            if (string.IsNullOrWhiteSpace(AwsCognitoUserPoolProviderKey)) { errors.Add(createVariableNotSetString(AwsCognitoUserPoolProviderKeyIdentifier)); }


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