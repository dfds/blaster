using System.Threading.Tasks;

namespace Blaster.WebApi
{
    public interface IApiKeyValidator
    {
        Task<bool> IsValid(string apiKey);
    }
}