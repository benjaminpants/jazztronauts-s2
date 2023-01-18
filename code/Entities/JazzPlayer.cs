using Jazztronauts.Data;
using Jazztronauts.Weapons;
using Sandbox;
using Player = Sandbox.Player;

namespace Jazztronauts.Entities;

public partial class JazzPlayer : Player
{
	private Data.Player _playerData;

	public Data.Player Data
	{
		get
		{
			return _playerData;
		}
	}

	public long Money {
		get
		{
			return Data.Earned - Data.Spent;
		}
	}

	[ClientRpc]
	public void UpdateClientData(long Earned, long Spent)
	{
		_playerData.Earned = Earned;
		_playerData.Spent = Spent;
	}

	public void UpdateClientDataEasy()
	{
		UpdateClientData(To.Single(Client),Data.Earned,Data.Spent);
	}

	public ClothingContainer Clothing = new();

	public JazzPlayer()
	{
		Inventory = new Inventory(this);
	}

	public JazzPlayer(IClient cl) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient(cl);
		if (IsServer)
		{
			_playerData = Database.GetPlayerData(cl.PlayerId);
			Database.SaveData(_playerData);
		}
	}

	public override void ClientSpawn()
	{
		_playerData = new Data.Player();
		Log.Info(_playerData);
	}

	public override void OnClientActive(IClient client)
	{
		base.OnClientActive(client);
		if (IsServer)
		{
			UpdateClientDataEasy();
		}
	}

	public void OnDisconnect()
	{
		if (!IsServer) return;
		if (_playerData != null)
		{
			Database.SaveData(_playerData);
			//GameServices.SubmitScore(Client.PlayerId, _playerData.StolenMapProps.CalculateWorth());
		}
	}

	public override void Respawn()
	{
		SetModel("models/citizen/citizen.vmdl");

		Controller = new JazzWalkController
		{
			WalkSpeed = 120,
			SprintSpeed = 240,
			DefaultSpeed = 120,
			AirAcceleration = 100,
			AutoJump = true

		};

		Animator = new StandardPlayerAnimator();

		CameraMode = new FirstPersonCamera();

		Clothing.DressEntity(this);

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		//Clothing.DressEntity( this );

		if (!JazztronautsGame.Rules.DisableSteal)
		{
			PropSnatcher ps = new();

			Inventory.Add(ps, true);

			Inventory.Add(new RunTool());

			Inventory.Add(new BusSpawner());

		}
		if (JazztronautsGame.Rules.IsHub)
		{
			Inventory.Add(new BusSpawner());
		}


		//Inventory.SetActiveSlot(0,false);

		base.Respawn();
	}

	public override void OnKilled()
	{
		base.OnKilled();

		((Weapon)Inventory.Active).OnUnequipt();

		Inventory.DeleteContents();
	}

	public Entity SearchForStealables()
	{
		TraceResult tr = Trace.Ray(EyePosition, EyePosition + EyeRotation.Forward * 120)
			.Ignore(this)
			.Run();

		// See if any of the parent entities are usable if we ain't.
		Entity ent = tr.Entity;
		while (ent.IsValid() && !JazzHelpers.CheckIfEntityIsValidStealable(ent))
		{
			ent = ent.Parent;
		}

		// Nothing found, try a wider search
		if (!JazzHelpers.CheckIfEntityIsValidStealable(ent))
		{
			tr = Trace.Ray(EyePosition, EyePosition + EyeRotation.Forward * 85)
			.Radius(2)
			.Ignore(this)
			.Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while (ent.IsValid() && !JazzHelpers.CheckIfEntityIsValidStealable(ent))
			{
				ent = ent.Parent;
			}
		}

		if (JazzHelpers.CheckIfEntityIsValidStealable(ent)) return ent;

		return null;
	}

	public void SimulateAnimation(PawnController controller)
	{
		if (controller == null)
			return;

		CitizenAnimationHelper animHelper = new CitizenAnimationHelper(this);

		if (ActiveChild is BaseCarriable carry)
		{
			carry.SimulateAnimator(animHelper);
		}
		else
		{
			animHelper.HoldType = CitizenAnimationHelper.HoldTypes.None;
			animHelper.AimBodyWeight = 0.5f;
		}
	}

	public override float FootstepVolume()
	{
		return Velocity.WithZ(0).Length.LerpInverse(0.0f, 200.0f) * 5.0f;
	}

	public override void Simulate(IClient cl)
	{
		base.Simulate(cl);

		if (LifeState != LifeState.Alive)
			return;

		var controller = GetActiveController();
		if (controller != null)
		{
			EnableSolidCollisions = !controller.HasTag("noclip");

			SimulateAnimation(controller);
		}

		TickPlayerUse();

		SimulateActiveChild(cl, Inventory.Active);

		if (Input.Pressed(InputButton.View))
		{
			if (CameraMode is ThirdPersonCamera)
			{
				CameraMode = new FirstPersonCamera();
			}
			else
			{
				CameraMode = new ThirdPersonCamera();
			}
		}

		if (Input.Pressed(InputButton.Slot1))
		{
			Inventory.SetActiveSlot(0, false);
		}

		if (Input.Pressed(InputButton.Slot2))
		{
			Inventory.SetActiveSlot(1, false);
		}

		if (Input.Pressed(InputButton.Slot3))
		{
			Inventory.SetActiveSlot(2, false);
		}
	}

	[Event.Hotload]
	private void HotReload()
	{
		if (_playerData != null)
		{
			Database.SaveData(_playerData);
			_playerData = Database.GetPlayerData(_playerData.SteamId);
			Database.SaveData(_playerData);
		}
	}
}