using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Topic
{
    [Route("api/topics")]
    [ApiController]
    public class TopicApiController : ControllerBase
    {
        private readonly ITopicClient _topicClient;

        public TopicApiController(ITopicClient topicClient)
        {
            _topicClient = topicClient;
        }

        [HttpGet("", Name = "GetAllTopics")]
        public async Task<ActionResult<TopicListResponse>> GetAll()
        {
            var topics = await _topicClient.GetAll();

            return topics ?? new TopicListResponse
            {
                Items = new string[0]
            };
        }

        [HttpPost("", Name = "CreateTopic")]
        public async Task<IActionResult> CreateCapability([FromBody] CreateTopicRequest input)
        {
            await _topicClient.CreateTopic(input);

            return NoContent();
        }
    }
}