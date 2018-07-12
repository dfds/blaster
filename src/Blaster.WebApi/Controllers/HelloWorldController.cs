using System;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Controllers
{
    [Route("")]
    public class HelloWorldController : ControllerBase
    {
        [Route("")]
        public ActionResult HelloWorld()
        {
            var message = Environment.GetEnvironmentVariable("blaster_message");

            if (string.IsNullOrWhiteSpace(message))
            {
                message = "hello world";
            }

            return Ok(message);
        }
    }
}
