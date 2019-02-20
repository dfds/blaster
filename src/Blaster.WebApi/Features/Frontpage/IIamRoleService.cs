using System.Net.Http;
using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Frontpage
{
    public interface IIamRoleService
    {
        Task<KubeConfig> GetDefaultKubeConfig();
    }

    public class IamRoleService : IIamRoleService
    {
        private readonly HttpClient _client;

        public IamRoleService(HttpClient client)
        {
            _client = client;
        }

        public async Task<KubeConfig> GetDefaultKubeConfig()
        {
            var response = await _client.GetAsync("api/configs/kubeconfig");

            response.EnsureSuccessStatusCode();

            return new KubeConfig
            {
                ContentType = response.Content.Headers.ContentType.MediaType,
                FileName = response.Content.Headers.ContentDisposition.FileName,
                Content = await response.Content.ReadAsByteArrayAsync()
            };
        }
    }

    public class KubeConfig
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }
}