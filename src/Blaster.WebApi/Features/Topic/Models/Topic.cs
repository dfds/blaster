using System;

namespace Blaster.WebApi.Features.Topic
{
    public class Topic
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Visibility {get;set;}
        public MessageExample[] MessageExamples {get; set;}
    }
}