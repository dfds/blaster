using System;
using System.Net;

namespace Blaster.WebApi.Features.Shared
{
	public class AlreadyJoinedException : Exception
	{
	}

	public class ContextAlreadyAddedException : Exception
	{
        
	}

	public class RecoverableUpstreamException : Exception
	{
		public RecoverableUpstreamException(
			HttpStatusCode httpStatusCode, 
			string message
		)
		{
			HttpStatusCode = httpStatusCode;
			Message = message;
		}

		public HttpStatusCode HttpStatusCode { get; }
		public override string Message { get; }
	}
    
	public class CapabilityValidationException : Exception
	{
		public CapabilityValidationException(string message) : base(message)
		{

		}
	}

	public class CapabilityTopicValidationException : Exception
	{
		public CapabilityTopicValidationException(string message) : base(message)
		{
            
		}
	}

	public class UnknownCapabilityException : Exception
	{

	}
	public class UnauthorizedException : Exception
	{
	}
}
