using System;
using Blaster.WebApi.Features.Dashboards;

namespace Blaster.Tests.Builders
{
    public class DashboardDetailItemBuilder
    {
        private string _id;
        private string _team;
        private string _name;
        private DateTime _lastModified;
        private string _content;

        public DashboardDetailItemBuilder()
        {
            _id = "1";
            _team = "foo team";
            _name = "foo board";
            _lastModified = new DateTime(2000, 1, 1);
            _content = "foo content";
        }

        public DashboardDetailItem Build()
        {
            return new DashboardDetailItem
            {
                Id = _id,
                Name = _name,
                Team = _team,
                Content = _content,
                LastModified = _lastModified
            };
        }
    }
}