using System;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Controllers
{
    [Route("")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        public ActionResult Get()
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
