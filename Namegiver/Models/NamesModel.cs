using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Namegiver.Models
{
	class NamesModel
	{
		static int lastId = -1;

		public IDbConnection Db { get; set; }

		private async Task<Name> GetName(int id)
		{
			return await Db.QueryFirstAsync<Name>(
				"SELECT [Id], [Text], [Accepted], [RejectedCount] FROM [dbo].[Name] WHERE [Id] = @id",
				new { id });
		}

		internal async Task<Name> GetRandomName()
		{
			Name name = await Db.QueryFirstOrDefaultAsync<Name>(
				"SELECT TOP 1 [Id], [Text], [Accepted], [RejectedCount] FROM [dbo].[Name] WHERE [Accepted] = 0 AND [Id] != @lastId ORDER BY CRYPT_GEN_RANDOM(4)",
				new { lastId });
			lastId = name.Id;
			return name;
		}

		internal async Task AcceptName(int id)
		{
			await Db.ExecuteAsync(
				"UPDATE [dbo].[Name] SET [Accepted] = 1 WHERE [Id] = @id",
				new { id });
		}

		internal async Task RejectName(int id)
		{
			await Db.ExecuteAsync(
				"UPDATE [dbo].[Name] SET [Accepted] = 0, [RejectedCount] = [RejectedCount] + 1 WHERE [Id] = @id",
				new { id });
		}
	}
}