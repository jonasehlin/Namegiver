using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Namegiver.Models
{
	class NamegiverContext : IDisposable
	{
		private readonly IDbConnection connection;

		internal NamesModel Names { get; }

		internal NamegiverContext(string connectionString)
		{
			connection = new SqlConnection(connectionString);
			Names = new NamesModel(connection);
		}

		internal static NamegiverContext CreateDefault(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DefaultConnection");
			string server = configuration.GetSection("NAMEGIVER_SERVER").Value;
			if (string.IsNullOrWhiteSpace(server))
				server = "(localhost)";
			connectionString = connectionString.Replace("{server}", server);
			return new NamegiverContext(connectionString);
		}

		public void Dispose()
		{
			if (connection != null)
			{
				connection.Dispose();
			}
		}
	}
}