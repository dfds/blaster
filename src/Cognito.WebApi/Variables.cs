using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cognito.WebApi
{
    public class Variables
    {
        private const string AwsAccessKeyEnvironmentVariable = "aws-cognito-access-key";
        private const string AwsCognitoSecretAccessKey = "aws-cognito-secret-access-key";
        public string AwsAccessKey { get; }
        public string AwsSecretAccessKey { get; }

        public Variables()
        {
            AwsAccessKey = Environment.GetEnvironmentVariable(AwsAccessKeyEnvironmentVariable);
            AwsSecretAccessKey = Environment.GetEnvironmentVariable(AwsCognitoSecretAccessKey);
        }

        public void Validate()
        {
            var errors = new List<string>();
            if(errors.Any() == false) {return;}
        }
    }
}