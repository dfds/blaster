using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;

namespace Blaster.Tests.TestDoubles
{
    public class StubDashboardRepository : IDashboardRepository
    {
        private readonly DashboardListItem[] _listResult;
        private readonly DashboardDetailItem _singleResult;

        public StubDashboardRepository(DashboardListItem[] listResult = null, DashboardDetailItem singleResult = null)
        {
            _listResult = listResult ?? new DashboardListItem[0];
            _singleResult = singleResult;
        }

        public Task<DashboardDetailItem> Get(string id)
        {
            return Task.FromResult(_singleResult);
        }

        public Task<IEnumerable<DashboardListItem>> GetAll()
        {
            return Task.FromResult(_listResult.AsEnumerable());
        }
    }
}