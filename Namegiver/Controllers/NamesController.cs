using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System.Threading.Tasks;

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

		private NamegiverContext CreateContext()
		{
			string connectionString = Configuration.GetConnectionString("DefaultConnection");
			connectionString = connectionString.Replace("{server}", Configuration.GetSection("NAMEGIVER_SERVER").Value);
			return new NamegiverContext(connectionString);
		}

		[HttpGet]
		[Route("")]
		[Route("random")]
		public async Task<ActionResult<Name>> GetRandomName()
		{
			using (var db = CreateContext())
			{
				return Ok(await db.Names.GetRandomName());
			}
		}

		[HttpPut]
		[Route("{id}/accept")]
		public async Task<ActionResult> AcceptName(int id)
		{
			using (var db = CreateContext())
			{
				await db.Names.AcceptName(id);
			}
			
			return NoContent();
		}

		[HttpPut]
		[Route("{id}/reject")]
		public async Task<ActionResult> RejectName(int id)
		{
			using (var db = CreateContext())
			{
				await db.Names.RejectName(id);
			}
			return NoContent();
		}

		[HttpPost]
		[Route("add")]
		public async Task<ActionResult> AddName([FromBody] Name name)
		{
			using (var db = CreateContext())
			{
				return Ok(await db.Names.AddName(name));
			}
		}
	}
}