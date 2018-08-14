using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Blaster.WebApi.Features.Dashboards
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DashboardListItem>> GetAll();
        Task<DashboardDetailItem> Get(string id);
    }

    public class DashboardRepository : IDashboardRepository
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly string _dashboardServiceUrl = "http://white-hill-2974.getsandbox.com";

        public async Task<IEnumerable<DashboardListItem>> GetAll()
        {
            var url = $"{_dashboardServiceUrl}/api/dashboards";

            var result = await Client.GetStringAsync(url);
            var response = JsonConvert.DeserializeObject<DashboardListResponse>(result);

            return response.Items.AsEnumerable();
        }

        public async Task<DashboardDetailItem> Get(string id)
        {
            var url = $"{_dashboardServiceUrl}/api/dashboards/{id}";

            var result = await Client.GetStringAsync(url);
            var response = JsonConvert.DeserializeObject<DashboardDetailItem>(result);

            return response;
        }
    }
}