using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Dashboards.Models;

namespace Blaster.Tests.TestDoubles
{
    public class StubDashboardService : IDashboardService
    {
        private readonly DashboardListItem[] _listResult;
        private readonly DashboardDetailItem _singleResult;

        public StubDashboardService(DashboardListItem[] listResult = null, DashboardDetailItem singleResult = null)
        {
            _listResult = listResult ?? new DashboardListItem[0];
            _singleResult = singleResult;
        }

        public Task<DashboardListResponse> GetAll()
        {
            return Task.FromResult(new DashboardListResponse
            {
                Items = _listResult,
                TotalCount = _listResult.Length
            });
        }

        public Task<DashboardDetailItem> Get(string id)
        {
            return Task.FromResult(_singleResult);
        }

        public Task<DashboardDetailItem> Create(DashboardInput input)
        {
            return Task.FromResult(_singleResult);
        }

        public Task<DashboardDetailItem> Update(DashboardInputWithId input)
        {
            return Task.FromResult(_singleResult);
        }

        public Task Delete(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}