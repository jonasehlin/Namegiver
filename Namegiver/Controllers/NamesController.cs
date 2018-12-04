using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Namegiver.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Namegiver.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NamesController : ControllerBase
	{
		private readonly IConfiguration Configuration;

		private readonly NamesModel Names;

		public NamesController(IConfiguration configuration)
		{
			Configuration = configuration;
			string connectionString = Configuration.GetConnectionString("DefaultConnection");
			// TODO: Dispose SqlConnection object
			Names = new NamesModel(new SqlConnection(connectionString));
		}

		[HttpGet]
		[Route("")]
		[Route("random")]
		public async Task<ActionResult<Name>> GetRandomName()
		{
			return Ok(await Names.GetRandomName());
		}

		[HttpPut]
		[Route("{id}/accept")]
		public async Task<ActionResult> AcceptName(int id)
		{
			await Names.AcceptName(id);
			return NoContent();
		}

		[HttpPut]
		[Route("{id}/reject")]
		public async Task<ActionResult> RejectName(int id)
		{
			await Names.RejectName(id);
			return NoContent();
		}
	}
}