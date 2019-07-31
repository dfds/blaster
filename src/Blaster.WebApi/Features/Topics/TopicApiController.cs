using System;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Topics.models;
using Microsoft.AspNetCore.Mvc;

namespace Blaster.WebApi.Features.Topics
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
        public async Task<ActionResult<TopicsResponse>> GetAll()
        {
            var topics = await _topicClient.GetAll();

            return topics ?? new TopicsResponse
            {
                Items = new Topic[0]
            };
        }

        [HttpGet("{id}", Name = "GetTopicById")]
        public async Task<ActionResult<Topic>> GetById(string id)
        {
            var topic = await _topicClient.GetById(id);

            if (topic != null)
            {
                return new ActionResult<Topic>(topic);
            }
            
            return new ActionResult<Topic>(NotFound());
        }

        [HttpGet("by-capability-id/{id}", Name = "GetTopicsByCapabilityId")]
        public async Task<ActionResult<TopicsResponse>> GetByCapabilityId(string id)
        {
            var topics = await _topicClient.GetByCapabilityId(id);

            if (topics != null)
            {
                return new ActionResult<TopicsResponse>(topics);
            }
            
            return new ActionResult<TopicsResponse>(NotFound());
        }
        
    }
}