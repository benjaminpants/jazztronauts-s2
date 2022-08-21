using Jazztronauts.UI;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using System.Linq;

namespace Jazztronauts;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public class Jazztronauts : Game
{
	private MainHUD _mainHud;

	public int GeneratedShards = 0;
	
	public Jazztronauts()
	{

		


		if (!IsClient) return;

		_mainHud = new MainHUD();
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	/// 

	public override void ClientJoined(Client client)
	{
		base.ClientJoined(client);


		// Create a pawn for this client to play with
		JazzPlayer player = new(client);

		player.Respawn();

		client.Pawn = player;
	}

	public override void Simulate(Client cl)
	{
		base.Simulate(cl);

		if (IsServer && GeneratedShards == 0)
		{
			int shardstogenerate = 5; //TODO: calculate ideal shard count for map size
			for (int i = 0; i < shardstogenerate; i++)
			{
				JazzShard model = new JazzShard();
				model.Position = JazzHelpers.GetRandomSpot((Vector3.Up * 64f)).Value;
				GeneratedShards++;
			}
		}
	}

	[Event.Hotload]
	public void HotloadUpdate()
	{
		if (!IsClient) return;

		_mainHud?.Delete();
		_mainHud = new MainHUD();
	}
}