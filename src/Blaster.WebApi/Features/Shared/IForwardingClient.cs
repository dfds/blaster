using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Blaster.WebApi.Features.Shared
{
	public interface IForwardingClient
	{
		Task ForwardHeader(string headerName, string headerValue);
	}
}