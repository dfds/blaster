using Blaster.WebApi.Features.Capabilities.Models;

namespace Blaster.Tests.Builders
{
    public class UserBuilder
    {
        private string _email;

        public UserBuilder()
        {
            _email = "bar";
        }

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public Member Build()
        {
            return new Member
            {
                Email = _email
            };
        }
    }
}