using System;
using System.Collections;
using System.Collections.Generic;

namespace Cognito.WebApi.Failures
{
    public class ValidationFailed : IFailure
    {
        private readonly Hashtable FailedValidations;

        public ValidationFailed()
        {
            FailedValidations = new Hashtable();
        }

        public void Add(string field, string message)
        {
            FailedValidations.Add(field, message);
        }

        public string Message
        {
            get
            {
                var messageLines = new List<string> {"The following validations failed"};

                foreach (DictionaryEntry failedValidation in FailedValidations)
                {
                    messageLines.Add($"'{failedValidation.Key}' {failedValidation.Value}");
                }

                return string.Join(Environment.NewLine, messageLines);
            }
        }
    }
}