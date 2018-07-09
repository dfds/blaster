using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using k8s;

namespace Blaster.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamespacesController : ControllerBase
    {
        private readonly IKubernetes _client;
        public NamespacesController (IKubernetes client) {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<string> results = new List<string>();

            try
            {
                var namespaceList = await _client.ListNamespaceAsync();

                foreach (var item in namespaceList.Items)
                {
                    results.Add(item.Metadata.Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            

            return Ok(results);
        }
    }
}
