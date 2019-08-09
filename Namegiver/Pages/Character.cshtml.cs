using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System.Threading.Tasks;

namespace Namegiver.Pages
{
	public class CharacterModel : PageModel
	{
		private readonly NamegiverContext context;

		public Name Name { get; set; }

		[BindProperty]
		public int NameId { get; set; }

		public int NameInfoId { get; set; }

		public CharacterModel(IConfiguration configuration)
		{
			context = new NamegiverContext(configuration);
		}

		public async Task OnGetAsync(int id)
		{
			NameInfoId = id;
			Name = await context.Names.GetName(id);
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			Name = await context.Names.GetName(NameInfoId);
			ReadForm();
			await context.Names.UpdateName(Name);

			return RedirectToPage("");
		}

		private void ReadForm()
		{
			foreach (var k in Request.Form)
			{
				if (k.Key.StartsWith("nameInput"))
				{

				}
			}
		}
	}
}