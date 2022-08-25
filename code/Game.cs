using System.Collections.Generic;
using System.Linq;
using Jazztronauts.Entities;
using Jazztronauts.UI;
using Sandbox;

namespace Jazztronauts;

public class JazztronautsGame : Game
{
	private MainHUD _mainHud;
	private IList<JazzPlayer> _players = new List<JazzPlayer>();

	public static JazztronautsGame Instance { get; private set; }

	public List<JazzPlayer> JazzPlayers 
	{
		get
		{
			return (List<JazzPlayer>)_players;
		}
	}

	private static JazzGameRules _gameRules;

	public static JazzGameRules Rules { 
		get
		{
			if (_gameRules == null)
			{
				_gameRules = (JazzGameRules)Entity.All.ToList().Find(e => (e is JazzGameRules));
				if (_gameRules == null)
				{
					_gameRules = new JazzGameRules();
				}
				return _gameRules;
			}
			else
			{
				return _gameRules;
			}
		} }

	public int GeneratedShards;

	public bool HasRunMapStartup = false;

	public JazztronautsGame()
	{
		Instance = this;

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
		client.Pawn = player;

		_players.Add(player);

		player.Respawn();
	}

	public override void ClientDisconnect(Client client, NetworkDisconnectionReason reason)
	{
		base.ClientDisconnect(client, reason);

		if (client.Pawn is JazzPlayer jazzPlayer)
		{
			_players.Remove(jazzPlayer);
			jazzPlayer.OnDisconnect();
		}
	}

	// TODO: Make this. Not stupid, AKA figure out if theres a function for calling stuff when the map loads.
	public override void Simulate(Client cl)
	{
		base.Simulate(cl);

		if (IsServer && !HasRunMapStartup)
		{
			HasRunMapStartup = true;
			int shardstogenerate = 0; //TODO: calculate ideal shard count for map size
			if (!Rules.IsHubOrStory)
			{
				shardstogenerate = Rules.ShardCount;
			}
			for (int i = 0; i < shardstogenerate; i++)
			{
				JazzShard model = new JazzShard();
				model.Position = JazzHelpers.GetRandomSpot((Vector3.Up * 64f)).Value;
				GeneratedShards++;
			}
		}
	}

	public override void Shutdown()
	{
		base.Shutdown();

		foreach (JazzPlayer jazzPlayer in _players)
		{
			jazzPlayer.OnDisconnect();
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