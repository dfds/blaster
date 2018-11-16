using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Blaster.WebApi.Security
{
    public class IdTokenAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public IdTokenAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Task<string> IdToken => _contextAccessor.HttpContext != null
            ? _contextAccessor.HttpContext.GetTokenAsync("id_token")
            : Task.FromResult<string>(null);
    }
}