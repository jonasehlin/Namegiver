using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Namegiver.Pages
{
	public class TopRejectedNamesModel : PageModel
	{
		public IEnumerable<Name> TopList { get; private set; }

		private readonly IConfiguration Configuration;

		public TopRejectedNamesModel(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public async Task OnGetAsync()
		{
			using (var db = new NamegiverContext(Configuration))
			{
				TopList = await db.Names.GetTopRejectedNames();
			}
		}
	}
}