using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Namespaces
{
    public interface INamespaceRepository
    {
        Task<IEnumerable<Namespace>> GetAll();
    }
}