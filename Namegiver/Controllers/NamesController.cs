using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Namegiver.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NamesController : ControllerBase
	{
		private readonly IConfiguration Configuration;

		public NamesController(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		[HttpGet]
		[Route(""), Route("random")]
		public async Task<ActionResult<Name>> GetRandomName()
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				return Ok(await db.Names.GetRandomName());
			}
		}

		[HttpPut]
		[Route("{id}/accept")]
		public async Task<ActionResult> AcceptName(int id)
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				await db.Names.AcceptName(id);
			}
			
			return NoContent();
		}

		[HttpPut]
		[Route("{id}/reject")]
		public async Task<ActionResult> RejectName(int id)
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				await db.Names.RejectName(id);
			}
			return NoContent();
		}

		[HttpPost]
		[Route("add")]
		public async Task<ActionResult<int>> AddName([FromBody] Name name)
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				return Ok(await db.Names.AddName(name));
			}
		}

		[HttpPut]
		[Route("{id}/reset")]
		public async Task<ActionResult> ResetName(int id)
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				await db.Names.ResetName(id);
			}
			return NoContent();
		}

		[HttpDelete]
		[Route("{id}/delete")]
		public async Task<ActionResult> DeleteName(int id)
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				await db.Names.DeleteName(id);
			}
			return NoContent();
		}

		[HttpGet]
		[Route("rejected")]
		public async Task< ActionResult<IEnumerable<Name>>> GetTopRejectedNames()
		{
			using (var db = NamegiverContext.CreateDefault(Configuration))
			{
				return Ok(await db.Names.GetTopRejectedNames());
			}
		}
	}
}