using Sandbox;
using System;
using System.Linq;

namespace Jazztronauts;

public class JazzShard : ModelEntity
{

	Vector3 BasePosition = Vector3.Zero;

	Sound? AmbienceSound;
	
	public JazzShard()
	{
	}

	public override void Spawn()
	{
		base.Spawn();

		SetModel("models/jazzshard.vmdl");
		

		Tags.Clear();

		UsePhysicsCollision = true;

		Tags.Add("trigger");

		AmbienceSound = Sound.FromWorld("shard.hum", Position);

	}

	public override void StartTouch(Entity other)
	{
		base.StartTouch(other);

		if (!IsServer) return;

		if (other is JazzPlayer)
		{
			Log.Info("Shard Collected!");
			Sound.FromWorld("rust_pumpshotgun.shootdouble", Position);
			Particles.Create("particles/explosion/barrel_explosion/explosion_barrel.vpcf", Position);
			Delete();
		}
	}

	public Vector3 GetWavyTranslation(float t)
	{
		return Vector3.Up * (float)Math.Sin(t) * 4f;
	}


	[Event.Tick.Server]
	protected void ServerUpdate()
	{
		

		if (BasePosition == Vector3.Zero)
		{
			BasePosition = Position;
			if (BasePosition == Vector3.Zero)
			{
				throw new Exception();
			}
		}
		float stime = Time.Now + (NetworkIdent * 15);
		//t, math.sin(t/2) * 360, math.cos(t/3) * 360
		Position = BasePosition + GetWavyTranslation(stime);
		Rotation = (new Angles(stime, (float)Math.Sin(stime / 2) * 360, (float)Math.Cos(stime / 3) * 360)).ToRotation();
	}
}