﻿using Sandbox;

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
}