using Jazztronauts.UI;
using Sandbox;
using Sandbox.UI;

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

	public Jazztronauts()
	{
		if (!IsClient) return;

		_mainHud = new MainHUD();
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined(Client client)
	{
		base.ClientJoined(client);

		// Create a pawn for this client to play with
		JazzPlayer player = new(client);

		player.Respawn();

		client.Pawn = player;
	}

	[Event.Hotload]
	public void HotloadUpdate()
	{
		if (!IsClient) return;

		_mainHud?.Delete();
		_mainHud = new MainHUD();
	}
}