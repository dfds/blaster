using System;

namespace Blaster.WebApi.Features.System
{
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string message) : base(message)
        {
            
        }
    }
}