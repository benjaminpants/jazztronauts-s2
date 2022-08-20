using Sandbox;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Jazztronauts;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public partial class Jazztronauts : Sandbox.Game
{
	public Jazztronauts()
	{
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var player = new JazzPlayer(client);

		player.Respawn();

		client.Pawn = player;
	}
}
