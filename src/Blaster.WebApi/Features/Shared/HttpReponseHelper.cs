using System;
using System.Net;
using System.Net.Http;

namespace Blaster.WebApi.Features.Shared
{
	public static class HttpResponseHelper
	{
		public static void EnsureSuccessStatusCode(HttpResponseMessage response)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthroizedException();
			}

			response.EnsureSuccessStatusCode();
		}
	}

	public class UnauthroizedException : Exception
	{
		public UnauthroizedException()
		{
		}
		
		public UnauthroizedException(string message) : base(message)
		{
		}
		
		public UnauthroizedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
