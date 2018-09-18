using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Blaster.WebApi.Controllers
{
    [Route("")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            _logger = logger;
        }

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

        [Route("api/values")]
        public ActionResult HelloWorld2()
        {
            _logger.LogInformation("Hejhej");
            return Ok("hejhej");
        }
    }
}
