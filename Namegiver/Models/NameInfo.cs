namespace Namegiver.Models
{
	public class NameInfo
	{
		public int Id { get; set; }
		public int NameId { get; set; }
		public string Name { get; set; }
		public bool Accepted { get; set; }
		public int RejectedCount { get; set; }
		public string Language { get; set; }
	}
}