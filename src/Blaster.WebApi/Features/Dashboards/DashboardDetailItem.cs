using System;

namespace Blaster.WebApi.Features.Dashboards
{
    public class DashboardDetailItem
    {
        public string Id { get; set; }
        public string Team { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }
        public string Content { get; set; }
    }
}