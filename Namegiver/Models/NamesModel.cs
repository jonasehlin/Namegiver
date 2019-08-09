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
		private static int lastInfoId = -1;

		private readonly IDbConnection db;

		internal NamesModel(IDbConnection db)
		{
			this.db = db;
		}

		public async Task<Name> GetName(int nameInfoId)
		{
			const string sqlGetName = @"
				SELECT n.[Id], n.[Gender], n.[SuperType], n.[Data]
					FROM [dbo].[NameInfo] ni
					JOIN [dbo].[Name] n ON n.[Id] = ni.[NameId]
					WHERE ni.[Id] = @nameInfoId";
			Name name = await db.QueryFirstAsync<Name>(sqlGetName, new { nameInfoId });

			const string sqlGetNameInfos = @"
					SELECT i.[Id], i.[NameId], i.[Name], i.[Accepted], i.[RejectedCount], i.[Language]
					FROM [dbo].[NameInfo] ni
					JOIN [dbo].[NameInfo] i ON i.[NameId] = ni.[NameId]
					WHERE ni.[Id] = @nameInfoId";
			name.Infos = await db.QueryAsync<NameInfo>(sqlGetNameInfos, new { nameInfoId });

			return name;
		}

		public async Task<Name> GetNameFromId(int nameId)
		{
			const string sqlGetName = @"
				SELECT n.[Id], n.[Gender], n.[SuperType], n.[Data]
					FROM [dbo].[Name] n
					WHERE n.[Id] = @nameId";
			Name name = await db.QueryFirstAsync<Name>(sqlGetName, new { nameId });

			const string sqlGetNameInfos = @"
					SELECT ni.[Id], ni.[NameId], ni.[Name], ni.[Accepted], ni.[RejectedCount], ni.[Language]
					FROM [dbo].[NameInfo] ni
					WHERE ni.[NameId] = @nameId";
			name.Infos = await db.QueryAsync<NameInfo>(sqlGetNameInfos, new { nameId });

			return name;
		}

		internal Task<string> GetNameInfoUrl(int nameId)
		{
			const string sql = "SELECT TOP 1 [Value] FROM [dbo].[NameProperty] WHERE [NameId] = @nameId";
			return db.ExecuteScalarAsync<string>(sql, new { nameId });
		}

		internal async Task<NameInfo> GetRandomNameInfo()
		{
			const string sql = @"
				SELECT TOP 1 [Id], [NameId], [Name]
				FROM [dbo].[NameInfo]
				WHERE [Accepted] = 0 AND [Id] != @lastInfoId
				ORDER BY CRYPT_GEN_RANDOM(4)";
			NameInfo info = await db.QueryFirstOrDefaultAsync<NameInfo>(sql, new { lastInfoId });
			lastInfoId = info.Id;
			return info;
		}

		internal async Task<NameInfo> GetNameInfo(int nameInfoId)
		{
			NameInfo info = await db.QueryFirstOrDefaultAsync<NameInfo>(@"
				SELECT TOP 1 [Id], [NameId], [Name]
				FROM [dbo].[NameInfo]
				WHERE [Id] = @nameInfoId",
				new { nameInfoId });
			if (info != null)
				lastInfoId = info.Id;
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

			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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
SELECT CAST(SCOPE_IDENTITY() AS INT)",
				name);
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

		internal async Task UpdateName(Name name)
		{
			if (db.State == ConnectionState.Open)
				db.Close();

			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				//Name name = await GetNameFromId(updatedName.Id);
				//if (name == null)
				//	throw new ArgumentException();

				const string nameSql = @"
					UPDATE [dbo].[Name] SET [Gender] = @Gender, [SuperType] = @SuperType, [Data] = @Data
					WHERE [Id] = @Id";
				int result = await db.ExecuteAsync(nameSql, name);
				if (result != 1)
					throw new ArgumentException($"There was an error updating the name. Name.Id: {name.Id}, result: {result}");

				foreach (var ni in name.Infos)
				{
					await CreateOrUpdateNameInfo(ni);
				}

				scope.Complete();
			}
		}

		private Task<int> CreateOrUpdateNameInfo(NameInfo ni)
		{
			const string sql = @"
				IF EXISTS (SELECT NULL FROM [dbo].[NameInfo] WHERE [Id] = @Id AND [NameId] = @NameId)
					UPDATE [dbo].[NameInfo]
					SET [Name] = @Name, [Accepted] = @Accepted, [RejectedCount] = @RejectedCount, [Language] = @Language
					WHERE [Id] = @Id AND [NameId] = @NameId
				ELSE
					SELECT 'TODO: Create new name info...'";
			return db.ExecuteAsync(sql);
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

		internal async Task<IEnumerable<NameInfo>> GetTopRejectedNames()
		{
			return await db.QueryAsync<NameInfo>(@"
SELECT TOP 10 [Id], [NameId], [Name], [RejectedCount]
FROM [dbo].[NameInfo]
WHERE [RejectedCount] > 0
ORDER BY [RejectedCount] DESC");
		}
	}
}