using System;
using Sandbox;
using SandboxEditor;

namespace Jazztronauts
{
	[Spawnable]
	[HammerEntity]
	[Library("jazz_gamerules", Title = nameof(JazzGameRules))]
	public class JazzGameRules : Entity
	{

		/// <summary>
		/// Is this a hub or story map? If it is a hub/story map, players won't spawn with stealing weapons.
		/// Shards also won't spawn.
		/// </summary>
		[Property(Title = "Is Hub or Story?")]
		public bool IsHubOrStory { get; set; } = false;

		/// <summary>
		/// How many shards should spawn on this map? This value is ignored if you've manually placed shard spawnpoints.
		/// </summary>
		[Property(Title = "Shard Count")]
		public int ShardCount { get; set; } = 5;
	}
}
