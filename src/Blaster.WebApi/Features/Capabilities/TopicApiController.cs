using System.Net.Http;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Capabilities
{
    [Route("api/defunct/topics")]
    [ApiController]
    public class TopicApiController : ControllerBase
    {
        private readonly ICapabilityServiceClient _capabilityServiceClient;

        public TopicApiController(ICapabilityServiceClient capabilityServiceClient)
        {
            _capabilityServiceClient = capabilityServiceClient;
        }

        [HttpGet("", Name = "GetAllTopics")]
        public async Task<ActionResult<TopicsResponse>> GetAll()
        {
            var topics = await _capabilityServiceClient.GetAllTopics();

            return topics ?? new TopicsResponse
            {
                Items = new Topic[0]
            };
        }

        [HttpGet("{topicId}", Name = "GetTopic")]
        public async Task<ActionResult<Topic>> Get(string topicId)
        {
            try
            {
                var topic = await _capabilityServiceClient.GetTopic(topicId);
                return new ActionResult<Topic>(topic);
            }
            catch (HttpRequestException)
            {
                return new ActionResult<Topic>(BadRequest());
            }
        }
        
        // UPDATE API CONTRACT
        [HttpPut("{topicId}", Name = "UpdateTopic")]
        public async Task<IActionResult> Update(string topicId, [FromBody] Topic input)
        {
            try
            {
                await _capabilityServiceClient.UpdateTopic(topicId, input);
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet("{topicId}/messageContracts", Name = "GetAllMessageContractsForTopic")]
        public async Task<ActionResult<MessageContractsResponse>> GetAllMessageContractsForTopic(string topicId)
        {
            var messageContracts = await _capabilityServiceClient.GetMessageContractsByTopicId(topicId);

            return messageContracts ?? new MessageContractsResponse
            {
                Items = new MessageContract[0]
            };
        }

        // TODO: Made obsolete in recent API contract revision, to be removed.
        [HttpPost("{topicId}/messageContracts", Name = "CreateMessageContract")]
        public async Task<IActionResult> CreateMessageContract(string topicId, [FromBody] MessageContractInput input)
        {
            try
            {
                await _capabilityServiceClient.CreateMessageContract(input.Type, input.Description, input.Content,
                    topicId);
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{topicId}/messageContracts/{type}", Name = "DeleteMessageContract")]
        public async Task<IActionResult> DeleteMessageContract(string topicId, string type)
        {
            try
            {
                await _capabilityServiceClient.RemoveMessageContract(topicId, type);
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("{topicId}/messageContracts/{type}", Name = "AddUpdateMessageContract")]
        public async Task<IActionResult> AddUpdateMessageContract(string topicId, string type, [FromBody] MessageContractInput input)
        {
            try
            {
                await _capabilityServiceClient.AddUpdateMessageContract(type, topicId, input);
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
