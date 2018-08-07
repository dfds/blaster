using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;

namespace Blaster.WebApi.Features.Namespaces
{
    public class NamespaceRepository : INamespaceRepository
    {
        private readonly IKubernetes _client;

        public NamespaceRepository(IKubernetes client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Namespace>> GetAll()
        {
            var namespaces = await _client.ListNamespaceAsync();

            return namespaces
                .Items
                .Select(x => new Namespace(
                    name: x.Metadata.Name,
                    createdDate: x.Metadata.CreationTimestamp.GetValueOrDefault(DateTime.MinValue)
                ));
        }
    }
}