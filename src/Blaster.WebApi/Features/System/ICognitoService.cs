using System.Threading.Tasks;

namespace Blaster.WebApi.Features.System
{
    public interface ICognitoService
    {
        Task<string> SayHello();
    }
}