namespace Namegiver.Models
{
	public class Name
	{
		public int Id { get; set; }
		public GenderEnum? Gender { get; set; }
		public SuperTypeEnum? SuperType { get; set; }

		public NameInfo[] Infos { get; set; }
	}
}