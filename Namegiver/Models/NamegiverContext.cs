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

		internal NamegiverContext(IConfiguration configuration)
		{
			string connectionString = configuration.GetSection("NAMEGIVER").Value;
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("ConnectionString is empty", nameof(connectionString));
			connection = new SqlConnection(connectionString);

			Names = new NamesModel(connection);
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