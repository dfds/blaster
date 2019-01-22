using System;
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
                var email = string.Empty;
                if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("X-User-Name", out var headerName))
                {
                    name = DecodeBase64(headerName);
                }

                if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("X-User-Email", out var headerEmail))
                {
                    email = DecodeBase64(headerEmail);
                }
                
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

        private string DecodeBase64(string base64String)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64String);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
            
            return decodedText;
        }
    }
}