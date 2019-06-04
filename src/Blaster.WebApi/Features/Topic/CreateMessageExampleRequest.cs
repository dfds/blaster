using System.ComponentModel.DataAnnotations;

namespace Blaster.WebApi.Features.Topic
{
    public class CreateMessageExampleRequest
    {
        [Required]
        public string MessageType { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}
