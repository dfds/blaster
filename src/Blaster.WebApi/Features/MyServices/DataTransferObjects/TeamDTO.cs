using System.Collections.Generic;

namespace DFDS.TeamService.WebApi.Features.UserServices.model
{
    public class TeamDTO
    {
        public string Name { get; set; }
        public string Department { get; set; }

        public IEnumerable<ServiceDTO> Services {get;set;}
    }
}