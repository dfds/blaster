using System.Threading.Tasks;
using Blaster.WebApi.Features.System.Models;

namespace Blaster.WebApi.Features.System
{
    public interface ICognitoService
    {
        Task<string> SayHello();
        Task<AwsConsoleLinkResponse> GetAwsConsoleLink(string idToken);
    }
}