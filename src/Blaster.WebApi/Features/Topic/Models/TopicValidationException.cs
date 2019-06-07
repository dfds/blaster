using System;

namespace Blaster.WebApi.Features.Topic
{
    public class TopicValidationException : Exception
    {
        public TopicValidationException(string message) : base(message){}
    }
}