using Jazztronauts.Entities;
using Sandbox;

namespace Jazztronauts.Weapons;

[Spawnable]
[Library("weapon_busspawner", Title = "BusSpawner")]
public class BusSpawner : Weapon
{
	public override string ViewModelPath => "";

	public override float PrimaryRate => 15f;
	public override float SecondaryRate => 2f;

	public override void Spawn()
	{
		base.Spawn();

		//SetModel("weapons/rust_pistol/rust_pistol.vmdl");
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed(InputButton.PrimaryAttack);
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

		if (!IsServer) return;

		Global.ChangeLevel("jazz_bar");

	}
}