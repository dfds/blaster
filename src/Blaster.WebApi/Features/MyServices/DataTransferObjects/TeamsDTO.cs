using System.Collections.Generic;

namespace DFDS.TeamService.WebApi.Features.UserServices.model
{
    public class TeamsDTO
    {
        public IEnumerable<TeamDTO> Items { get; set; }
    }
}