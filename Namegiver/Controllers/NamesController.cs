﻿using Microsoft.AspNetCore.Mvc;
using Namegiver.Models;
using System.Collections.Generic;
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
			names.Db = new SqlConnection("Data Source=(local);Initial Catalog=Namegiver;Integrated Security=True");
		}

		[HttpGet]
		[Route("")]
		[Route("random")]
		public async Task<ActionResult<Name>> GetRandomName()
		{
			return await names.GetRandomName();
		}

		[HttpGet("{id}", Name = "Get")]
		public string Get(int id)
		{
			return "value";
		}

		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}