namespace Namegiver.Models
{
	public class Name
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public bool Accepted { get; set; }
		public int RejectCount { get; set; }
	}
}