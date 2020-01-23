using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;

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

		public static async Task MapStatusCodeToException(HttpResponseMessage response)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthroizedException();
			}

			if (HttpStatusCode.Conflict == response.StatusCode ||
			    HttpStatusCode.UnprocessableEntity == response.StatusCode)
			{
				var payload = await response.Content.ReadAsStringAsync();
				throw new RecoverableUpstreamException(response.StatusCode, payload);
			}
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
