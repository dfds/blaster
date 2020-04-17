using System;
using System.Collections.Generic;

namespace Blaster.WebApi.Features.Capabilities.Models
{
	public class CreateTopicRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Partitions { get; set; }
		public bool DryRun { get; set; }
		public Dictionary<string, object> Configurations { get; set; }
	}
}
