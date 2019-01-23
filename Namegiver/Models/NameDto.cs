using System.Collections.Generic;

namespace Namegiver.Models
{
	public class NameDto
	{
		public Name Name { get; set; }
		public IEnumerable<NameInfo> Infos { get; set; }
	}
}