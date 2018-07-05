using System.Threading.Tasks;
using Blaster.WebApi;

namespace Blaster.Tests.TestDoubles
{
    public class StubApiKeyValidator : IApiKeyValidator
    {
        private readonly bool _isValid;

        public StubApiKeyValidator(bool isValid)
        {
            _isValid = isValid;
        }
        public Task<bool> IsValid(string apiKey)
        {
            return Task.FromResult(_isValid);
        }
    }
}