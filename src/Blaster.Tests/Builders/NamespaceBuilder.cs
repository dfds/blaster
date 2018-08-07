using System;
using Blaster.WebApi.Features.Namespaces;

namespace Blaster.Tests.Builders
{
    public class NamespaceBuilder
    {
        private DateTime _createdDate;
        private string _name;

        public NamespaceBuilder()
        {
            _name = "foo";
            _createdDate = new DateTime(2000, 1, 1);
        }

        public Namespace Build()
        {
            return new Namespace(_name, _createdDate);
        }
    }
}