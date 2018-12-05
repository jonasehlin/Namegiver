using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Namegiver.Pages
{
	public class AddNameModel : PageModel
	{
		private readonly IConfiguration Configuration;

		[BindProperty]
		public string Name { get; set; }

		[BindProperty]
		public string Status { get; set; }

		public AddNameModel(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private NamegiverContext CreateContext()
		{
			string connectionString = Configuration.GetConnectionString("DefaultConnection");
			connectionString = connectionString.Replace("{server}", Configuration.GetSection("NAMEGIVER_SERVER").Value);
			return new NamegiverContext(connectionString);
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				try
				{
					using (var db = CreateContext())
					{
						int newId = await db.Names.AddName(new Name() { Text = Name });
						Status = $"\"{Name}\" successfully added with id: {newId}";
					}
				}
				catch (SqlException ex)
				{
					Trace.WriteLine(ex.ToString());
					Status = $"Database error: {ex.Message}";
				}
				catch (ArgumentException ex)
				{
					Trace.WriteLine(ex.ToString());
					Status = $"Error: {ex.Message}";
				}
			}

			return Page();
		}
	}
}