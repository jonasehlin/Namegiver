namespace Namegiver.Models
{
	public class Name
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public bool Accepted { get; set; }
		public int RejectedCount { get; set; }
		public string Language { get; set; }
		public byte Gender { get; set; }
	}
}