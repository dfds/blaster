using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Namespaces
{
    public class NamespaceRepository : INamespaceRepository
    {
        public Task<IEnumerable<Namespace>> GetAll()
        {
            throw new Exception();

            var list = new[]
            {
                new Namespace(),
                new Namespace(),
                new Namespace(),
            };

            return Task.FromResult(list.AsEnumerable());
        }
    }
}