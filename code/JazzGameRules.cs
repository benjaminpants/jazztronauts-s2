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
		/// Players won't spawn with stealing weapons if this is true. Shards also won't spawn.
		/// </summary>
		[Property(Title = "No Stealing Map")]
		public bool DisableSteal { get; set; } = false;

		/// <summary>
		/// If this map is a hub, they'll spawn with the bus spawner but it'll send everyone to a random map. This is placeholder behavior.
		/// </summary>
		[Property(Title = "Is Hub")]
		public bool IsHub { get; set; } = false;

		/// <summary>
		/// How many shards should spawn on this map? This value is ignored if you've manually placed shard spawnpoints.
		/// </summary>
		[Property(Title = "Shard Count")]
		public int ShardCount { get; set; } = 5;
	}
}
