namespace Blaster.WebApi.Features.Dashboards
{
    public class DashboardListResponse
    {
        public DashboardListItem[] Items { get; set; }
        public long TotalCount { get; set; }
    }
}