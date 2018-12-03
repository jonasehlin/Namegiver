using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Namegiver.Models
{
	class NamesModel
	{
		static int lastId = -1;

		public IDbConnection Db { get; set; }

		private async Task<IEnumerable<int>> GetAvailableIds()
		{
			return await Db.QueryAsync<int>(
				"SELECT [Id] FROM [dbo].[Name] WHERE [Accepted] = 0 AND [Id] != @lastId",
				new { lastId });
		}

		private async Task<Name> GetName(int id)
		{
			return await Db.QueryFirstAsync<Name>(
				"SELECT [Id], [Text], [Accepted], [RejectedCount] FROM [dbo].[Name] WHERE [Id] = @id",
				new { id });
		}

		internal async Task<Name> GetRandomName()
		{
			IList<int> availableIds = (await GetAvailableIds()).ToList();

			var rand = new Random();
			int id;
			do
			{
				// Get random id but not same as last id
				int index = rand.Next(availableIds.Count());
				id = availableIds[index];
			} while (availableIds.Count() > 1 && lastId == id);

			lastId = id;
			return await GetName(id);
		}

		internal async Task AcceptName(int id)
		{
			await Db.ExecuteAsync(
				"UPDATE [dbo].[Name] SET [Accepted] = 1 WHERE [Id] = @id",
				new { id });
		}
	}
}