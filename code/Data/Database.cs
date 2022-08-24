using Jazztronauts.Data.Interfaces;
using Sandbox;

namespace Jazztronauts.Data;

internal class Database
{
	private const string PLAYER_DATA_PATH = "jazztronauts/playerdata";

	public static Player GetPlayerData(long steamId)
	{
		return FileSystem.Data.ReadJsonOrDefault($"{PLAYER_DATA_PATH}/{steamId}.json", new Player(steamId));
	}

	public static void SaveData<TGameData>(TGameData gameData) where TGameData : IGameData
	{
		switch (gameData)
		{
			case Player playerData:
				SavePlayerData(playerData);
				break;
		}
	}

	private static void SavePlayerData(Player playerData)
	{
		FileSystem.Data.WriteJson($"{PLAYER_DATA_PATH}/{playerData.SteamId}.json", playerData);
	}

	//public static void Test()
	//{
	//	FileSystem.Data.CreateDirectory(PLAYER_DATA_PATH);
	//	using Stream str = FileSystem.Data.OpenWrite($"{PLAYER_DATA_PATH}/fasguy.json");
	//	//// Open database (or create if doesn't exist)
	//	//using LiteDatabase db = new(str);
	//	//// Get a collection (or create, if doesn't exist)
	//	//ILiteCollection<Player> col = db.GetCollection<Player>("players");

	//	//// Create your new customer instance
	//	//Player customer = new();

	//	//// Insert new customer document (Id will be auto-incremented)
	//	//col.Insert(customer);

	//	//// Update a document inside a collection
	//	//customer.Name = "Jane Doe";

	//	//col.Update(customer);

	//	//// Index document using document Name property
	//	//col.EnsureIndex(x => x.Name);

	//	//// Use LINQ to query documents (filter, sort, transform)
	//	//var results = col.Query()
	//	//	.Where(x => x.Name.StartsWith("J"))
	//	//	.OrderBy(x => x.Name)
	//	//	.Select(x => new { x.Name, NameUpper = x.Name.ToUpper() })
	//	//	.Limit(10)
	//	//	.ToList();

	//	//// Let's create an index in phone numbers (using expression). It's a multikey index
	//	//col.EnsureIndex(x => x.Phones);

	//	//// and now we can query phones
	//	//Player r = col.FindOne(x => x.Phones.Contains("8888-5555"));
	//}
}