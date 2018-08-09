using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blaster.WebApi.Features.Dashboards
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DashboardListItem>> GetAll();
        Task<DashboardDetailItem> Get(string id);
    }
}