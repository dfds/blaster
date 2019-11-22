using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Blaster.WebApi.Features.Shared
{
	// This filter expects a public method called 'ForwardHeaders' with no parameters that also returns void, on the attached Controller Class.
	// Take a look at CapabilityApiController for an example of how this is used.
	public class ForwardHeader : ActionFilterAttribute
	{
		public const string XMsalAuthTokenHeader = "X-Msal-Auth-Token";
		public const string AuthorizationHeader = "Authorization";
		public static void Forward(HttpRequest request, IForwardingClient client, string headerName, string headerForwardedName, string prependedText = "")
		{
			StringValues authToken = StringValues.Empty;
			request.Headers.TryGetValue(headerName, out authToken);
			
			if (authToken.Any())
			{
				client.ForwardHeader(headerForwardedName, $"{prependedText}{authToken.First()}").Wait();
			}
		}

		public static void ForwardMsal(HttpRequest request, IForwardingClient client)
		{
			Forward(
				request: request, 
				client: client,
				headerName: XMsalAuthTokenHeader, 
				headerForwardedName: AuthorizationHeader,
				prependedText: "Bearer ");
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var controller = (ControllerBase)context.Controller;
			if (controller == null)
			{
				return;
			}

			var protectedRequestMethod = controller.GetType().GetMethod("ForwardHeaders");
			if (protectedRequestMethod == null)
			{
				return;
			}

			protectedRequestMethod.Invoke(controller, null);
			
			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
		}
	}
}
