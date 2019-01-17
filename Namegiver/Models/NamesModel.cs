using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

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

		internal async Task<NameInfo> GetRandomNameInfo()
		{
			NameInfo info = await db.QueryFirstOrDefaultAsync<NameInfo>(@"
SELECT TOP 1 [Id], [Name]
FROM [dbo].[NameInfo]
WHERE [Accepted] = 0 AND [Id] != @lastId ORDER BY CRYPT_GEN_RANDOM(4)",
				new { lastId });
			lastId = info.Id;
			return info;
		}

		internal async Task AcceptName(int id)
		{
			await db.ExecuteAsync(
				"UPDATE [dbo].[NameInfo] SET [Accepted] = 1 WHERE [Id] = @id",
				new { id });
		}

		internal async Task RejectName(int id)
		{
			await db.ExecuteAsync(
				"UPDATE [dbo].[NameInfo] SET [Accepted] = 0, [RejectedCount] = [RejectedCount] + 1 WHERE [Id] = @id",
				new { id });
		}

		internal async Task<int> AddName(Name name)
		{
			if (db.State == ConnectionState.Open)
				db.Close();

			using (var scope = new TransactionScope())
			{
				name.Id = await CreateName(name);

				foreach (var info in name.Infos)
				{
					info.NameId = name.Id;
					info.Id = await CreateNameInfo(info);
				}

				scope.Complete();
			}

			return name.Id;
		}

		private async Task<int> CreateName(Name name)
		{
			int? nameId = await db.ExecuteScalarAsync<int?>(@"
INSERT INTO [dbo].[Name] ([Gender], [SuperType])
VALUES (@Gender, @SuperType)
SELECT CAST(SCOPE_IDENTITY() AS INT)");
			if (nameId <= 0)
				throw new ArgumentException("Could not create a Name row", nameof(name));
			return nameId.Value;
		}

		private async Task<int> CreateNameInfo(NameInfo info)
		{
			info.Name = info.Name.Trim();

			if (string.IsNullOrEmpty(info.Name))
				throw new ArgumentException("Name must not be empty", nameof(info.Name));

			int? infoId = await db.ExecuteScalarAsync<int?>(@"
INSERT INTO [dbo].[NameInfo] ([NameId], [Name], [Language])
VALUES (@NameId, @Name, @Language)
SELECT CAST(SCOPE_IDENTITY() as INT)",
				info);
			if (infoId <= 0)
				throw new ArgumentException("Could not create a NameInfo row", nameof(info));
			return infoId.Value;
		}

		internal async Task ResetName(int id)
		{
			await db.ExecuteAsync(
				"UPDATE [dbo].[Name] SET [Accepted] = 0, [RejectedCount] = 0 WHERE [Id] = @id",
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