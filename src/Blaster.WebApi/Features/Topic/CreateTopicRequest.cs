using System.ComponentModel.DataAnnotations;

namespace Blaster.WebApi.Features.Topic
{
    public class CreateTopicRequest
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Visibility { get; set; }
    }
}