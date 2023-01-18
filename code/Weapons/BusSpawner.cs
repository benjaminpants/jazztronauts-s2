using Jazztronauts.Entities;
using Sandbox;

namespace Jazztronauts.Weapons;

[Spawnable]
[Library("weapon_busspawner", Title = "BusSpawner")]
public class BusSpawner : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15f;
	public override float SecondaryRate => 2f;

	public BusSummon MySummon;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("weapons/rust_pistol/rust_pistol.vmdl");
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed(InputButton.PrimaryAttack);
	}

	public override void Simulate(IClient owner)
	{
		base.Simulate(owner);

		if (MySummon != null)
		{
			if (!Input.Down(InputButton.PrimaryAttack))
			{
				MySummon.PlayersAiming--;
				if (MySummon.PlayersAiming <= 0)
				{
					MySummon.Delete();
				}
				MySummon = null;
			}
		}
	}

	public override void SimulateAnimator(CitizenAnimationHelper anim)
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
		anim.Handedness = CitizenAnimationHelper.Hand.Right;
		anim.AimBodyWeight = 1f;
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

	}


	public override void AttackPrimary() //TODO: Implement actual bus summoning
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if (!Game.IsServer) return;

		Trace trace = Trace.Ray(((JazzPlayer)Owner).EyePosition, ((JazzPlayer)Owner).EyePosition + (((JazzPlayer)Owner).EyeRotation.Forward * 500f))
			.UseHitboxes()
			.WithAnyTags("solid", "bus")
			.Ignore(this)
			.Size(2);

		TraceResult tr = trace.Run();

		if (tr.Entity == null) return;

		if (tr.Entity.ClassName == "BusSummon")
		{
			MySummon = (BusSummon)tr.Entity;
			MySummon.PlayersAiming++;
		}

		if (tr.Entity.ClassName != "worldent") return;

		BusSummon mden = new BusSummon();

		mden.PlayersAiming = 1;

		mden.Position = tr.EndPosition;

		mden.Owner = this;

		mden.Rotation = Rotation.LookAt(tr.Normal * -1, Vector3.Zero);

		MySummon = mden;

		//Global.ChangeLevel("jazz_bar");

	}
}