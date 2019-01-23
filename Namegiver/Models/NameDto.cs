using System.Collections.Generic;

namespace Namegiver.Models
{
	public class NameDto
	{
		public Name Name { get; set; }
		public string InfoUrl { get; set; }
		public string ImageUrl { get; set; }
		public IEnumerable<NameInfo> NameInfos { get; set; }
	}
}