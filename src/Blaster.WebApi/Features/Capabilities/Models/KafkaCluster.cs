namespace Blaster.WebApi.Features.Capabilities.Models
{
	public class KafkaCluster
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Enabled { get; set; }
		public string ClusterId { get; set; }
	}
}
