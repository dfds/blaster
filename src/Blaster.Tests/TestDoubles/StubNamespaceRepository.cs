using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Controllers;
using Blaster.WebApi.Features.Namespaces;

namespace Blaster.Tests.TestDoubles
{
    public class StubNamespaceRepository : INamespaceRepository
    {
        private readonly Namespace[] _result;

        public StubNamespaceRepository(params Namespace[] result)
        {
            _result = result;
        }

        public Task<IEnumerable<Namespace>> GetAll()
        {
            return Task.FromResult(_result.AsEnumerable());
        }
    }
}