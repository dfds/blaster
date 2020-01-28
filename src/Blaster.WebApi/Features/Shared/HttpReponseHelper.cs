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
				throw new UnauthorizedException();
			}

			response.EnsureSuccessStatusCode();
		}

		public static async Task MapStatusCodeToException(HttpResponseMessage response)
		{
			switch (response.StatusCode)
			{
				case HttpStatusCode.Unauthorized:
					throw new UnauthorizedException();
				case HttpStatusCode.Conflict:
				case HttpStatusCode.UnprocessableEntity:
				{
					var payload = await response.Content.ReadAsStringAsync();
					throw new RecoverableUpstreamException(response.StatusCode, payload);
				}
			}
		}
	}
}
