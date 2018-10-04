using System;
using Cognito.WebApi.Failures;

namespace Cognito.WebApi
{
    public class Result<TSuccessResult, TFailureResult>
    {
        private readonly TSuccessResult _successResult;
        private readonly TFailureResult _failureResult;

        public Result(TSuccessResult successResult)
        {
            _successResult = successResult;
        }

        public Result(TFailureResult failureResult) 
        {
            _failureResult = failureResult;
        }

        
        public void Handle(
            Action<TSuccessResult> successFunction,
            Action<TFailureResult> failureFunction
        )
        {
            if (_failureResult != null)
            {
                failureFunction(_failureResult);

                return;
            }
            successFunction(_successResult);
        }
        
              
        public TOutput Reduce<TOutput>(
            Func<TSuccessResult, TOutput> successFunction,
            Func<TFailureResult, TOutput> failureFunction
        )
            => _failureResult != null ?
                failureFunction(_failureResult) :
                successFunction(_successResult);
    }
}