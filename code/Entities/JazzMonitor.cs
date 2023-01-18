using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Jazztronauts.Weapons;

namespace Jazztronauts.Entities //TODO: work on this
{

	[Spawnable]
	[EditorModel("models/light_arrow.vmdl")]
	[Library("jazz_monitor", Title = nameof(JazzMonitor))]
	public partial class JazzMonitor : ModelEntity, IUse
	{



		public JazzMonitor()
		{
		}

		public bool IsUsable(Entity user)
		{
			return true;
		}

		public bool OnUse(Entity user)
		{
			return true;
		}

		public override void Spawn()
		{
			base.Spawn();
			SetModel("models/light_arrow.vmdl");
			SetupPhysicsFromModel(PhysicsMotionType.Dynamic);
		}
	}
}
