using System;

namespace Blaster.WebApi.Features.Capabilities.Models
{
	public class CreateTopicRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Partitions { get; set; }
		public long RetentionPeriodInDays { get; set; }
		public bool DryRun { get; set; }
	}
}
