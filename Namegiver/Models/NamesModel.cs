using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Namegiver.Models
{
	class NamesModel
	{
		static int lastId = -1;

		private readonly IDbConnection db;

		internal NamesModel(IDbConnection db)
		{
			this.db = db;
		}

		private async Task<Name> GetName(int id)
		{
			return await db.QueryFirstAsync<Name>(
				"SELECT [Id], [Text], [Accepted], [RejectedCount] FROM [dbo].[Name] WHERE [Id] = @id",
				new { id });
		}

		internal async Task<Name> GetRandomName()
		{
			Name name = await db.QueryFirstOrDefaultAsync<Name>(
				"SELECT TOP 1 [Id], [Text], [Accepted], [RejectedCount] FROM [dbo].[Name] WHERE [Accepted] = 0 AND [Id] != @lastId ORDER BY CRYPT_GEN_RANDOM(4)",
				new { lastId });
			lastId = name.Id;
			return name;
		}

		internal async Task AcceptName(int id)
		{
			await db.ExecuteAsync(
				"UPDATE [dbo].[Name] SET [Accepted] = 1 WHERE [Id] = @id",
				new { id });
		}

		internal async Task RejectName(int id)
		{
			await db.ExecuteAsync(
				"UPDATE [dbo].[Name] SET [Accepted] = 0, [RejectedCount] = [RejectedCount] + 1 WHERE [Id] = @id",
				new { id });
		}

		internal async Task<int> AddName(Name name)
		{
			if (string.IsNullOrWhiteSpace(name.Text))
				throw new ArgumentException("Need a name", nameof(name.Text));

			return (await db.QueryFirstAsync<int?>(@"
INSERT INTO [dbo].[Name] ([Text]) VALUES (@Text)
SELECT CAST(SCOPE_IDENTITY() as INT)",
				new { Text = name.Text.Trim() }
				)).Value;
		}

		internal async Task ResetName(int id)
		{
			await db.ExecuteAsync(
				"UPDATE SET [Accepted] = 0, [RejectedCount] = 0 WHERE [Id] = @id",
				new { id });
		}

		internal async Task DeleteName(int id)
		{
			await db.ExecuteAsync(
				"DELETE FROM [dbo].[Name] WHERE [Id] = @id",
				new { id });
		}

		internal async Task<IEnumerable<Name>> GetTopRejectedNames()
		{
			return await db.QueryAsync<Name>(@"
SELECT TOP 10 *
FROM [dbo].[Name]
WHERE [RejectedCount] > 0
ORDER BY [RejectedCount] DESC");
		}
	}
}