using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System.Threading.Tasks;

namespace Namegiver.Pages
{
	public class CharacterModel : PageModel
	{
		private readonly NamegiverContext context;

		public NameDto Name { get; set; }

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
	}
}