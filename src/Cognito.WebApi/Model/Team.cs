using System.Collections.Generic;

namespace Cognito.WebApi.Model
{
    public class Team
    {
        public string Name { get; set; }
        public List<User> Members { get; set; }
    }
}