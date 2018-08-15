using Blaster.WebApi.Features.Dashboards.Models;

namespace Blaster.Tests.Builders
{
    public class DashboardInputBuilder
    {
        private string _name;
        private string _team;
        private string _content;

        public DashboardInputBuilder()
        {
            _name = "foo";
            _team = "bar";
            _content = "baz-qux";
        }

        public DashboardInput Build()
        {
            return new DashboardInput
            {
                Name = _name,
                Team = _team,
                Content = _content
            };
        }
    }
}