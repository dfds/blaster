using System;

namespace Blaster.WebApi.Features.Namespaces
{
    public class Namespace
    {
        public Namespace(string name, DateTime createdDate)
        {
            Name = name;
            CreatedDate = createdDate;
        }

        public string Name { get; }
        public DateTime CreatedDate { get; }
    }
}