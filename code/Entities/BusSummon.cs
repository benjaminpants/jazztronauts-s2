using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Jazztronauts.Weapons;

namespace Jazztronauts.Entities
{
	public partial class BusSummon : ModelEntity
	{

		[Net]
		public int PlayersAiming { get; set; }


		public BusSummon()
		{
		}

		[Event.Tick.Server]
		public void ServerTick()
		{
			if (PlayersAiming == JazztronautsGame.Instance.JazzPlayers.Count)
			{
				if (JazztronautsGame.Rules.IsHub)
				{
					_ = JazzHelpers.GoToRandomMap();
				}
				else
				{
					Game.ChangeLevel("jazz_bar");
				}
			}
		}


		public override void Spawn()
		{
			base.Spawn();
			SetModel("models/light_arrow.vmdl");
			SetupPhysicsFromModel(PhysicsMotionType.Dynamic);
			Tags.Add("bus");
		}
	}
}
