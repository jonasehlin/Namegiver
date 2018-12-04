using Microsoft.AspNetCore.Mvc;
using Namegiver.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Namegiver.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NamesController : ControllerBase
	{
		NamesModel names { get; } = new NamesModel();

		public NamesController()
		{
			// TODO: Inject connection from services
			names.Db = new SqlConnection("Data Source=hobgoblin;Initial Catalog=Namegiver;Integrated Security=True");
		}

		[HttpGet]
		[Route("")]
		[Route("random")]
		public async Task<ActionResult<Name>> GetRandomName()
		{
			return Ok(await names.GetRandomName());
		}

		[HttpPut]
		[Route("{id}/accept")]
		public async Task<ActionResult> AcceptName(int id)
		{
			await names.AcceptName(id);
			return NoContent();
		}

		[HttpPut]
		[Route("{id}/reject")]
		public async Task<ActionResult> RejectName(int id)
		{
			await names.RejectName(id);
			return NoContent();
		}
	}
}