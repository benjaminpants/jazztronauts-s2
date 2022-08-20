using Sandbox;

namespace Jazztronauts;

[Spawnable]
[Library("weapon_propsnatcher", Title = "PropSnatcher")]
public class PropSnatcher : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("weapons/rust_pistol/rust_pistol.vmdl");
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed(InputButton.PrimaryAttack);
	}

	public override void SimulateAnimator(CitizenAnimationHelper anim)
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
		anim.Handedness = CitizenAnimationHelper.Hand.Right;
		anim.AimBodyWeight = 1.0f;
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		PlaySound("rust_pistol.shoot");

		Vector3 forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;


		foreach (TraceResult tr in TraceMelee(Owner.EyePosition, Owner.EyePosition + forward * 80, 70f))
		{
			if (!IsServer) continue;

			Entity ent = tr.Entity;

			using (Prediction.Off())
			{
				if (JazzHelpers.CheckIfEntityIsValidStealable(ent))
				{
					if (ent is not ModelEntity animent) return;
					CollectedProp cp = new(animent);
					((JazzPlayer)Owner).Money += JazzHelpers.CalculateModelWorth(animent.Model);
					ent.Delete();
				}
			}
		}

		(Owner as AnimatedEntity)?.SetAnimParameter("b_attack", true);
	}
}