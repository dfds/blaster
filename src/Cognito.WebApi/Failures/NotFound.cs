namespace Cognito.WebApi.Failures
{
    public class NotFound :IFailure
    {
        public NotFound(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}