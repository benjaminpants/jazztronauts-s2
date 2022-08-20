using Sandbox;

namespace Jazztronauts;

[Spawnable]
[Library( "weapon_propsnatcher", Title = "PropSnatcher" )]
public partial class PropSnatcher : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.PrimaryAttack );
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
		anim.Handedness = CitizenAnimationHelper.Hand.Right;
		anim.AimBodyWeight = 1.0f;
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		bool didsnatch = false;
		
		
		foreach (var tr in TraceMelee(Owner.EyePosition, Owner.EyePosition + (forward * 80), 70f))
		{

			var ent = tr.Entity;

			if ( JazzHelpers.CheckIfEntityIsValidStealable( ent ) )
			{
				didsnatch = true;
			}

			if ( !IsServer ) continue;


			using ( Prediction.Off() )
			{
				if ( JazzHelpers.CheckIfEntityIsValidStealable(ent))
				{
					var animent = (ent as ModelEntity);
					if ( animent == null) return;
					var cp = new CollectedProp(animent);
					(Owner as JazzPlayer).Money += JazzHelpers.CalculateModelWorth(animent.Model);
					ent.Delete();
				}
			}

		}

		if ( didsnatch )
		{
			PlaySound( "snatch_get" );
		}
		else
		{
			PlaySound( "snatch_miss" );
		}

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
	}
}
