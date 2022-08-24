using Jazztronauts.Data.Interfaces;

namespace Jazztronauts.Data;

public class Player : IGameData
{
	public Player() { }

	public Player(long steamId)
	{
		SteamId = steamId;
	}

	public long SteamId { get; set; }

	public long Earned { get; set; }

	public long Spent { get; set; }

	public long Resets { get; set; }
}