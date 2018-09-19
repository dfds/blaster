using System.Collections;

namespace Cognito.WebApi.Failures
{
    public class ValidationFailed :IFailure
    {
        private readonly Hashtable FailedValidations;

        public ValidationFailed()
        {
            FailedValidations = new Hashtable();
        }

        public void Add(string field, string message)
        {
            FailedValidations.Add(field,message);
        }
        
        public string Message
        {
            get { return ""; }
        }
    }
}