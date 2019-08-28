using System.Collections.Generic;

namespace Blaster.WebApi.Features.Capabilities.Models
{
    public class Topic
    {
        public string Id { get; set; }
        public string CapabilityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string NameBusinessArea { get; set; }
        public string NameType { get; set; }
        public string NameMisc { get; set; }
        public IEnumerable<MessageContract> MessageContracts { get; set; }
    }
}

