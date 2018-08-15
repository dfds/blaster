using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards.Models;

namespace Blaster.WebApi.Features.Dashboards
{
    public interface IDashboardService
    {
        Task<DashboardListResponse> GetAll();
        Task<DashboardDetailItem> Get(string id);
        Task<DashboardDetailItem> Create(DashboardInput input);
        Task<DashboardDetailItem> Update(DashboardInputWithId input);
        Task Delete(string id);
    }
}