using System;

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
            if (_successResult != null)
            {
                successFunction(_successResult);
                return;
            }

            failureFunction(_failureResult);
        }
        
        
        public TOutput Reduce<TOutput>(
            Func<TSuccessResult, TOutput> successFunction,
            Func<TFailureResult, TOutput> failureFunction
        )
            => _successResult != null ?
                successFunction(_successResult) :
                failureFunction(_failureResult);
    }
}