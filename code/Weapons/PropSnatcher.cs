using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Jazztronauts;

[Spawnable]
[Library("weapon_propsnatcher", Title = "PropSnatcher")]
public class PropSnatcher : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15f;
	public override float SecondaryRate => 2f;

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
		anim.HoldType = CitizenAnimationHelper.HoldTypes.HoldItem;
		anim.Handedness = CitizenAnimationHelper.Hand.Right;
		anim.AimBodyWeight = 0.5f;
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		//iterate through every sceneobject. I can't find a way to actually check if the entites these represent are world objects or not. But this DOES contain static props.
		
		List<SceneObject> list = Map.Scene.SceneObjects.ToList();

		foreach (SceneObject item in list)
		{
			item.SetMaterialOverride(Material.Load("materials/jazz_void.vmat"));
		}
		
	}


	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		Vector3 forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		bool didsnatch = false;


		foreach (TraceResult tr in TraceMelee(Owner.EyePosition, Owner.EyePosition + forward * 80, 70f))
		{
			Entity ent = tr.Entity;

			if (JazzHelpers.CheckIfEntityIsValidStealable(ent))
			{
				didsnatch = true;
			}

			if (!IsServer) continue;


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

		if (didsnatch)
		{
			PlaySound("snatch_get");
		}
		else
		{
			PlaySound("snatch_miss");
		}

		(Owner as AnimatedEntity)?.SetAnimParameter("b_attack", true);
	}
}