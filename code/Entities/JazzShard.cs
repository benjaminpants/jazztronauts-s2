using Sandbox;
using System;
using System.Linq;

namespace Jazztronauts;

public class JazzShard : ModelEntity
{

	Vector3 BasePosition = Vector3.Zero;

	Sound AmbienceSound;

	PointLightEntity Light;
	
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

		Light = new PointLightEntity
		{
			Enabled = true,
			Color = Color.White,
			DynamicShadows = false,
			Range = 256,
			Falloff = 1.0f,
			LinearAttenuation = 0.0f,
			QuadraticAttenuation = 1.0f,
			Brightness = 1f
		};

		//AmbienceSound = Sound.FromEntity("shard.hum", this);

	}

	public override void ClientSpawn()
	{
		AmbienceSound = Sound.FromEntity("shard.hum", this);
	}

	public override void StartTouch(Entity other)
	{
		base.StartTouch(other);
		bool TouchedPlayer = (other is JazzPlayer);
		if (TouchedPlayer && IsClient)
		{
			AmbienceSound.SetVolume(0f);
			AmbienceSound.Stop();
		}
		if (!IsServer) return;

		if (TouchedPlayer)
		{
			Log.Info("Shard Collected!");
			Sound.FromWorld("rust_pumpshotgun.shootdouble", Position);
			Particles.Create("particles/explosion/barrel_explosion/explosion_barrel.vpcf", Position);
			Light.Delete();
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
			Light.Position = BasePosition;
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