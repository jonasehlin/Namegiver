namespace Namegiver.Models
{
	public class Name
	{
		public int Id { get; set; }
		public byte? Gender { get; set; }
		public SuperTypeEnum? SuperType { get; set; }

		public NameInfo[] Infos { get; set; }
	}
}