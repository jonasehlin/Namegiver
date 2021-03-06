﻿using Microsoft.AspNetCore.Mvc;
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

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				try
				{
					using (var db = new NamegiverContext(Configuration))
					{
						var name = new Name()
						{
							Infos = new[] { new NameInfo() { Name = this.Name } }
						};
						int newId = await db.Names.AddName(name);
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