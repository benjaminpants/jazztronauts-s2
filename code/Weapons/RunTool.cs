using Jazztronauts.Entities;
using Sandbox;

namespace Jazztronauts.Weapons;

[Spawnable]
[Library("weapon_runtool", Title = "RunTool")]
public class RunTool : Weapon
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
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
		anim.Handedness = CitizenAnimationHelper.Hand.Both;
		anim.AimBodyWeight = 2f;
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		
	}


	public override void OnEquipt()
	{
		JazzPlayer ply = (JazzPlayer)Owner;

		JazzWalkController controller = (JazzWalkController)ply.Controller;

		controller.WalkSpeed *= 2f;
		controller.DefaultSpeed *= 2f;
		controller.SprintSpeed *= 2f;
		controller.JumpMultiplier = 2.5f;
	}

	public override void OnUnequipt()
	{
		JazzPlayer ply = (JazzPlayer)Owner;

		JazzWalkController controller = (JazzWalkController)ply.Controller;

		controller.WalkSpeed /= 2f;
		controller.DefaultSpeed /= 2f;
		controller.SprintSpeed /= 2f;
		controller.JumpMultiplier = 1f;
	}


	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

	}
}