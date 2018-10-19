using System.Collections.Generic;

namespace Blaster.WebApi.Features.MyServices.Model
{
    public class Team
    {
        public string Name { get; set; }
        public string Department { get; set; }

        public IEnumerable<Service> Services {get;set;}
    }
}