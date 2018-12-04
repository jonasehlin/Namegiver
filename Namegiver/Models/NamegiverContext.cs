using System;
using System.Data;
using System.Data.SqlClient;

namespace Namegiver.Models
{
	class NamegiverContext : IDisposable
	{
		private readonly IDbConnection connection;

		public NamesModel Names { get; }

		public NamegiverContext(string connectionString)
		{
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