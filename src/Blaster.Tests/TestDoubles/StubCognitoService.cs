using System.Threading.Tasks;
using Blaster.WebApi.Features.System;

namespace Blaster.Tests.TestDoubles
{
    public class StubCognitoService : ICognitoService
    {
        public Task<string> SayHello()
        {
            return Task.FromResult("foo");
        }
    }
}