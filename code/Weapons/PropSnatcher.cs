using System.Collections.Generic;
using System.Linq;
using Jazztronauts.Data;
using Jazztronauts.Entities;
using Sandbox;

namespace Jazztronauts.Weapons;

[Spawnable]
[Library("weapon_propsnatcher", Title = nameof(PropSnatcher))]
public partial class PropSnatcher : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 10f;

	public override float SecondaryRate => 2f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("weapons/rust_pistol/rust_pistol.vmdl");
	}

	public override void Reload()
	{
		//no.
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack();
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
		
		List<SceneObject> list = Scene.SceneObjects.ToList();

		foreach (SceneObject item in list)
		{
			//item.SetMaterialOverride(Material.Load("materials/jazz_void.vmat"));
			item.ColorTint = Color.Random;
		}
	}

	public override void AttackPrimary()
	{
		if (Owner is not JazzPlayer playerEntity) return;

		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		Vector3 forward = playerEntity.EyeRotation.Forward;
		forward = forward.Normal;

		bool didsnatch = false;

		foreach (TraceResult tr in TraceMelee(playerEntity.EyePosition, playerEntity.EyePosition + forward * 80, 70f))
		{
			Entity ent = tr.Entity;

			if (JazzHelpers.CheckIfEntityIsValidStealable(ent))
			{
				didsnatch = true;
			}

			if (!Game.IsServer) continue;


			using (Prediction.Off())
			{
				if (JazzHelpers.CheckIfEntityIsValidStealable(ent))
				{
					if (ent is not ModelEntity animent) return;
					CollectedProp cp = new(animent);
					//playerEntity.Data.Earned += JazzHelpers.CalculateModelWorth(animent.Model);
					//playerEntity.UpdateClientDataEasy();
					playerEntity.Data.AddStolenProp(animent.Model);
					ent.Delete();
				}
			}

		}

		if (didsnatch)
		{
			PlaySound("snatcher.get");
		}
		else
		{
			PlaySound("snatcher.miss");
		}

		playerEntity.SetAnimParameter("b_attack", true);
	}
}