using Jazztronauts.Data.Interfaces;
using Sandbox;
using System.Collections.Generic;

namespace Jazztronauts.Data;

public class Player : IGameData
{
	public Player() 
	{
		StolenMapProps = new List<StolenProps>();
	}

	public Player(long steamId) : this()
	{
		SteamId = steamId;
	}

	public void AddStolenProp(Model mdl)
	{
		StolenProps props = StolenMapProps.Find(s => s.ModelPath == mdl.ResourcePath);
		if (props != null)
		{
			props.Count++;
		}
		else
		{
			StolenMapProps.Add(new StolenProps(mdl));
		}
	}

	public long SteamId { get; set; }

	public long Earned { get; set; }

	public long Spent { get; set; }

	public long Resets { get; set; }

	public List<StolenProps> StolenMapProps { get; set; }

}