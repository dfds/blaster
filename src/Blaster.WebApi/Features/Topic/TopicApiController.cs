using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Topic
{
    [Route("api/topics")]
    [ApiController]
    public class TopicApiController : ControllerBase
    {
        private readonly ITikaTopicClient _tikaTopicClient;
        private readonly ITopicClient _topicClient;
        
        public TopicApiController(
            ITikaTopicClient tikaTopicClient, 
            ITopicClient topicClient
        )
        {
            _tikaTopicClient = tikaTopicClient;
            _topicClient = topicClient; 
        }

        [HttpGet("", Name = "GetAllTopics")]
        public async Task<ActionResult<TopicListResponse>> GetAll()
        {
            var topics = await _tikaTopicClient.GetAll();

            return topics ?? new TopicListResponse
            {
                Items = new string[0]
            };
        }

        [HttpPost("", Name = "CreateTopic")]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest input)
        {
            await _tikaTopicClient.CreateTopic(input);

            return NoContent();
        }
        
        [HttpPost("{name}/messageexamples", Name = "CreateMessageExample")]
        public async Task<IActionResult> CreateMessageExample(
            string name, 
            [FromBody] CreateMessageExampleRequest input
        )
        {
            await _topicClient.CreateMessageExample(
                name, 
                input.MessageType,
                input.Text
            );
            
            return NoContent();
        }

        [HttpGet("{name}", Name = "GetTopicByName")]
        public async Task<ActionResult<Topic>> GetByName(string name)
        {
            var topic = await _topicClient.GetByName(name);

            if (topic != null)
            {
                return new ActionResult<Topic>(topic);
            }

            return new ActionResult<Topic>(NotFound());

        }
    }
}