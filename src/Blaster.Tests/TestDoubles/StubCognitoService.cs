using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.System.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubCognitoService : ICognitoService
    {
        public Task<string> SayHello()
        {
            return Task.FromResult("foo");
        }

        public Task<AwsConsoleLinkResponse> GetAwsConsoleLink(Guid teamId, string idToken)
        {
            return Task.FromResult(new AwsConsoleLinkResponse
            {
                AbsoluteUrl = "http://dr.dk"
            });
        }
    }
}