using System;

namespace Blaster.WebApi.Features.Topic
{
    public class Topic
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Visibility {get;set;}
        public MessageExample[] MessageExamples {get; set;}
    }
}