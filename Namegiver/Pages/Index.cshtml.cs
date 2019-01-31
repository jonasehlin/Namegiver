using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Namegiver.Pages
{
	public class IndexModel : PageModel
	{
		public int? NameInfoId { get; set; }

		public void OnGet(int? id)
		{
			NameInfoId = id;
		}
	}
}