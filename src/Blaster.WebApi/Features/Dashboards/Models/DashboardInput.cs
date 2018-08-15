namespace Blaster.WebApi.Features.Dashboards.Models
{
    public class DashboardInput
    {
        public string Team { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class DashboardInputWithId : DashboardInput
    {
        public string Id { get; set; }
    }
}