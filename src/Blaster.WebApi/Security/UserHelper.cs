using Microsoft.AspNetCore.Http;

namespace Blaster.WebApi.Security
{
    public class UserHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public User CurrentUser
        {
            get
            {
                var name = "[unknown user]";
                if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("X-User-Name", out var headerName))
                {
                    name = headerName;
                }

                _contextAccessor.HttpContext.Request.Headers.TryGetValue("X-User-Email", out var email);

                return new User(name, email);
            }
        }

        public class User
        {
            public User(string name, string email)
            {
                Name = name;
                Email = email;
            }

            public string Name { get; private set; }
            public string Email { get; private set; }
        }
    }
}