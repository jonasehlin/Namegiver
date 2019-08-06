using Newtonsoft.Json;
using System.Collections.Generic;

namespace Namegiver.Models
{
	public class Name
	{
		public int Id { get; set; }
		public GenderEnum? Gender { get; set; }
		public SuperTypeEnum? SuperType { get; set; }
		public string Data
		{
			get { return JsonConvert.SerializeObject(Properties); }
			set { Properties = JsonConvert.DeserializeObject<NameProperties>(value); }
		}

		[JsonIgnore]
		public NameProperties Properties { get; set; } = new NameProperties();

		[JsonIgnore]
		public IEnumerable<NameInfo> Infos { get; set; }
	}
}