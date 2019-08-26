using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Blaster.WebApi.Security
{
    public class UserHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHostingEnvironment _environment;

        public UserHelper(IHttpContextAccessor contextAccessor, IHostingEnvironment environment)
        {
            _contextAccessor = contextAccessor;
            _environment = environment;
        }

        public User CurrentUser => _environment.IsDevelopment()
            ? StubUser
            : GetUserFromHeaders();

        private User StubUser => new User("John Doe", "jdog@me.com");

        private User GetUserFromHeaders()
        {
            var name = "[unknown user]";
            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("X-User-Name", out var headerName))
            {
                name = DecodeBase64(headerName);
            }

            var email = string.Empty;
            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("X-User-Email", out var headerEmail))
            {
                email = DecodeBase64(headerEmail);
            }

            return new User(name, email.ToLower());
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

        private string DecodeBase64(string base64String)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64String);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

            return decodedText;
        }
    }
}